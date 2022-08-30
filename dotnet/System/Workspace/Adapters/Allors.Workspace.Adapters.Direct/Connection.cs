// <copyright file="LocalWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using Allors.Database.Services;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Meta;
    using Allors.Database;
    using Allors.Database.Meta;
    using Allors.Database.Security;
    using Allors.Shared.Ranges;
    using System.Linq;
    using System;
    using IOperandType = Meta.IOperandType;
    using System.Threading.Tasks;

    public class Connection : Adapters.Connection
    {
        private readonly Dictionary<long, Grant> accessControlById;
        private readonly ConcurrentDictionary<long, DatabaseRecord> recordsById;

        public Connection(IDatabase database, string name, MetaPopulation metaPopulation) : base(name, metaPopulation)
        {
            this.Database = database;

            this.recordsById = new ConcurrentDictionary<long, DatabaseRecord>();
            this.accessControlById = new Dictionary<long, Grant>();
        }

        public long UserId { get; set; }

        internal IDatabase Database { get; }

        internal void Sync(IEnumerable<IObject> objects, IAccessControl accessControl)
        {
            using (var transaction = this.Database.CreateTransaction())
            {
                foreach (var @object in objects)
                {
                    var id = @object.Id;
                    var databaseClass = @object.Strategy.Class;
                    var roleTypes = databaseClass.DatabaseRoleTypes.Where(w => w.RelationType.WorkspaceNames.Length > 0);

                    var workspaceClass = (Class)this.MetaPopulation.FindByTag(databaseClass.Tag);
                    var roleByRoleType = roleTypes.ToDictionary(w => ((RelationType)this.MetaPopulation.FindByTag(w.RelationType.Tag)).RoleType, w => this.GetRole(@object, w));

                    var acl = accessControl[@object];

                    var accessControls = acl.Grants?.Select(v => (IGrant)transaction.Instantiate(v.Id)).Select(this.GetAccessControl).ToArray() ?? Array.Empty<Grant>();

                    this.recordsById[id] = new DatabaseRecord(workspaceClass, id, @object.Strategy.ObjectVersion, roleByRoleType, ValueRange<long>.Load(acl.Revocations.Select(v => v.Id)), accessControls);
                }
            }
        }

        public override Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null) =>
           this.InvokeAsync(new[] { method }, options);

        public override Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null)
        {
            var workspace = new Workspace(this);
            var result = new Invoke(workspace);
            result.Execute(methods, options);
            return Task.FromResult<IInvokeResult>(result);
        }

        public override Task<IPullResult> CallAsync(Data.Procedure procedure, params Data.Pull[] pull)
        {
            var workspace = new Workspace(this);
            var result = new Pull(workspace);

            result.Execute(procedure);
            result.Execute(pull);

            workspace.OnPulled(result);

            return Task.FromResult<IPullResult>(result);
        }

        public override Task<IPullResult> CallAsync(object args, string name)
        {
            var workspace = new Workspace(this);
            var result = new Pull(workspace);

            result.Execute(args, name);

            return Task.FromResult<IPullResult>(result);
        }

        public override Task<IPullResult> PullAsync(params Data.Pull[] pulls)
        {
            foreach (var pull in pulls)
            {
                if (pull.ObjectId < 0 || pull.Object?.Id < 0)
                {
                    throw new ArgumentException($"Id is not in the database");
                }
            }

            var workspace = new Workspace(this);
            var result = new Pull(workspace);
            result.Execute(pulls);

            workspace.OnPulled(result);

            return Task.FromResult<IPullResult>(result);
        }

        public override Adapters.DatabaseRecord GetRecord(long id)
        {
            this.recordsById.TryGetValue(id, out var databaseObjects);
            return databaseObjects;
        }

        public override long GetPermission(Class workspaceClass, IOperandType operandType, Operations operation)
        {
            var @class = (Database.Meta.IClass)this.Database.MetaPopulation.FindByTag(workspaceClass.Tag);
            var operandId = this.Database.MetaPopulation.FindByTag(operandType.OperandTag).Id;

            long permission;
            switch (operation)
            {
                case Operations.Read:
                    @class.ReadPermissionIdByRelationTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Write:
                    @class.WritePermissionIdByRelationTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Execute:
                    @class.ExecutePermissionIdByMethodTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Create:
                    throw new NotSupportedException("Create is not supported");
                default:
                    throw new ArgumentOutOfRangeException($"Unknown operation {operation}");
            }

            return permission;
        }

        internal IEnumerable<IObject> ObjectsToSync(Pull pull) =>
            pull.DatabaseObjects.Where(v =>
            {
                if (this.recordsById.TryGetValue(v.Id, out var databaseRoles))
                {
                    return v.Strategy.ObjectVersion != databaseRoles.Version;
                }

                return true;
            });

        private Grant GetAccessControl(IGrant grant)
        {
            if (!this.accessControlById.TryGetValue(grant.Strategy.ObjectId, out var acessControl))
            {
                acessControl = new Grant();
                this.accessControlById.Add(grant.Strategy.ObjectId, acessControl);
            }

            if (acessControl.Version == grant.Strategy.ObjectVersion)
            {
                return acessControl;
            }

            acessControl.Version = grant.Strategy.ObjectVersion;
            acessControl.PermissionIds = ValueRange<long>.Import(grant.Permissions.Select(v => v.Id));

            return acessControl;
        }

        private object GetRole(IObject @object, IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return @object.Strategy.GetUnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return @object.Strategy.GetCompositeRole(roleType)?.Id;
            }

            return ValueRange<long>.Load(@object.Strategy.GetCompositesRole<IObject>(roleType).Select(v => v.Id));
        }
    }
}

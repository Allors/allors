// <copyright file="v.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Database.Meta.Extensions;

namespace Allors.Workspace.Adapters.Direct
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Security;
    using Database.Services;
    using Meta;
    using Shared.Ranges;
    using AccessControl = AccessControl;
    using IRoleType = Database.Meta.IRoleType;

    public class Connection : Adapters.Connection
    {
        private readonly Dictionary<long, AccessControl> accessControlById;
        private readonly ConcurrentDictionary<long, Adapters.Record> recordById;

        private readonly Func<IWorkspaceServices> servicesBuilder;

        public Connection(Configuration configuration, IDatabase database, Func<IWorkspaceServices> servicesBuilder) : base(configuration, new IdGenerator())
        {
            this.Database = database;
            this.servicesBuilder = servicesBuilder;

            this.recordById = new ConcurrentDictionary<long, Adapters.Record>();
            this.accessControlById = new Dictionary<long, AccessControl>();
        }

        public long UserId { get; set; }

        private IDatabase Database { get; }

        public IDatabaseServices DatabaseServices => this.Database.Services;

        public Database.Meta.MetaPopulation MetaPopulation => this.Database.MetaPopulation;

        public ITransaction CreateTransaction()
        {
            var transaction = this.Database.CreateTransaction();
            var user = (IUser)transaction.Instantiate(this.UserId);
            transaction.Services.Get<IUserService>().User = user;
            return transaction;
        }

        internal void Sync(IEnumerable<IObject> objects, IAccessControl accessControl)
        {
            using (var transaction = this.Database.CreateTransaction())
            {
                foreach (var @object in objects)
                {
                    var id = @object.Id;
                    var databaseClass = @object.Strategy.Class;
                    var roleTypes = databaseClass.RoleTypes.Where(w => w.RelationType.WorkspaceNames.Any());

                    var workspaceClass = (IClass)this.Configuration.MetaPopulation.FindByTag(databaseClass.Tag);
                    var roleByRoleType = roleTypes.ToDictionary(w => ((IRelationType)this.Configuration.MetaPopulation.FindByTag(w.RelationType.Tag)).RoleType, w => this.GetRole(@object, w));

                    var acl = accessControl[@object];

                    var accessControls = acl.Grants?.Select(v => (IGrant)transaction.Instantiate(v.Id)).Select(this.GetAccessControl).ToArray() ?? Array.Empty<AccessControl>();

                    this.recordById[id] = new Record(workspaceClass, id, @object.Strategy.ObjectVersion, roleByRoleType, ValueRange<long>.Load(acl.Revocations.Select(v => v.Id)), accessControls);
                }
            }
        }

        public override IWorkspace CreateWorkspace() => new Workspace(this, this.servicesBuilder());

        public override Adapters.Record GetRecord(long id)
        {
            this.recordById.TryGetValue(id, out var record);
            return record;
        }

        public override long GetPermission(IClass workspaceClass, IOperandType operandType, Operations operation)
        {
            var @class = (Database.Meta.IClass)this.Database.MetaPopulation.FindByTag(workspaceClass.Tag);
            var operandId = this.Database.MetaPopulation.FindByTag(operandType.OperandTag).Id;

            long permission;
            switch (operation)
            {
                case Operations.Read:
                    @class.ReadPermissionIdByRelationTypeId().TryGetValue(operandId, out permission);
                    break;
                case Operations.Write:
                    @class.WritePermissionIdByRelationTypeId().TryGetValue(operandId, out permission);
                    break;
                case Operations.Execute:
                    @class.ExecutePermissionIdByMethodTypeId().TryGetValue(operandId, out permission);
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
                if (this.recordById.TryGetValue(v.Id, out var databaseRoles))
                {
                    return v.Strategy.ObjectVersion != databaseRoles.Version;
                }

                return true;
            });

        private AccessControl GetAccessControl(IGrant grant)
        {
            if (!this.accessControlById.TryGetValue(grant.Strategy.ObjectId, out var acessControl))
            {
                acessControl = new AccessControl();
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

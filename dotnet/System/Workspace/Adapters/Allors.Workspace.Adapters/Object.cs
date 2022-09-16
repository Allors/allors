// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Response;
    using Shared.Ranges;

    public abstract class Object : IObject, IComparable<Object>
    {
        private readonly long rangeId;

        protected Object(Workspace workspace, Record record)
        {
            this.Workspace = workspace;
            this.Record = record;
            this.Id = record.Id;
            this.rangeId = this.Id;
            this.Class = record.Class;
        }

        public Workspace Workspace { get; }

        public Record Record { get; private set; }

        protected IEnumerable<RoleType> RoleTypes => this.Class.DatabaseOriginRoleTypes;

        int IComparable<Object>.CompareTo(Object other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.rangeId.CompareTo(other.rangeId);
        }

        IWorkspace IObject.Workspace => this.Workspace;

        public Class Class { get; }

        public long Id { get; }

        public bool ExistRole(RoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType) != null;
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole(roleType) != null;
            }

            return this.GetCompositesRole(roleType).Any();
        }

        public object GetRole(RoleType roleType)
        {
            if (roleType == null)
            {
                throw new ArgumentNullException(nameof(roleType));
            }

            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole(roleType);
            }

            return this.GetCompositesRole(roleType);
        }


        public object GetUnitRole(RoleType roleType) => this.Record?.GetRole(roleType);

        IObject IObject.GetCompositeRole(RoleType roleType) => this.GetCompositeRole(roleType);

        IEnumerable<IObject> IObject.GetCompositesRole(RoleType roleType) => this.GetCompositesRole(roleType);

        public long Version => this.Record?.Version ?? Allors.Version.WorkspaceInitial;

        public bool CanRead(RoleType roleType)
        {
            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Read);
            return this.Record.IsPermitted(permission);
        }

        public bool CanWrite(RoleType roleType)
        {
            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Write);
            return this.Record.IsPermitted(permission);
        }

        public bool CanExecute(MethodType methodType)
        {
            var permission = this.Workspace.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this.Record.IsPermitted(permission);
        }

        public Object GetCompositeRole(RoleType roleType)
        {
            var role = this.Record?.GetRole(roleType);

            if (role == null)
            {
                return null;
            }

            var @object = this.Workspace.GetObject((long)role);
            this.AssertObject(@object);
            return @object;
        }

        public RefRange<Object> GetCompositesRole(RoleType roleType)
        {
            var role = (ValueRange<long>)(this.Record?.GetRole(roleType) ?? ValueRange<long>.Empty);

            if (role.IsEmpty)
            {
                return RefRange<Object>.Empty;
            }

            return RefRange<Object>.Load(role.Select(v =>
            {
                var @object = this.Workspace.GetObject(v);
                this.AssertObject(@object);
                return @object;
            }));
        }

        private void AssertObject(Object @object)
        {
            if (@object == null)
            {
                throw new Exception("Object is not in Workspace.");
            }
        }

        public void OnPulled(IPullResult pull)
        {
            var newRecord = this.Workspace.Connection.GetRecord(this.Id);
            this.Record = newRecord;
        }
    }
}

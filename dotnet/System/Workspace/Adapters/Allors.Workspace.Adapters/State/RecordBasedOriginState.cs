// <copyright file="RecordBasedOriginState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Shared.Ranges;

    public abstract class RecordBasedOriginState
    {
        public abstract Object Object { get; }

        protected RecordBasedOriginState(Record record) => this.Record = record;

        public object GetUnitRole(RoleType roleType) => this.Record?.GetRole(roleType);

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

        #region Proxy Properties

        protected long Id => this.Object.Id;

        protected Class Class => this.Object.Class;

        protected Workspace Workspace => this.Object.Workspace;

        protected Connection Connection => this.Workspace.Connection;

        #endregion

        public long Version => this.Record?.Version ?? Allors.Version.WorkspaceInitial;

        protected IEnumerable<RoleType> RoleTypes => this.Class.DatabaseOriginRoleTypes;

        // TODO: Remove
        protected bool ExistRecord => this.Record != null;

        protected Record Record { get; private set; }

        public bool CanRead(RoleType roleType)
        {
            if (!this.ExistRecord)
            {
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Read);
            return this.Record.IsPermitted(permission);
        }

        public bool CanWrite(RoleType roleType)
        {
            if (!this.ExistRecord)
            {
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Write);
            return this.Record.IsPermitted(permission);
        }

        public bool CanExecute(MethodType methodType)
        {
            if (!this.ExistRecord)
            {
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this.Record.IsPermitted(permission);
        }

        public void OnPulled(IPullResultInternals pull)
        {
            var newRecord = this.Workspace.Connection.GetRecord(this.Id);
            this.Record = newRecord;
        }
    }
}

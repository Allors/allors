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

    public abstract class Strategy : IObject, IComparable<Strategy>
    {
        private readonly long rangeId;

        protected Strategy(Workspace workspace, Class @class, long id)
        {
            this.Workspace = workspace;
            this.Id = id;
            this.rangeId = this.Id;
            this.Class = @class;
        }

        protected Strategy(Workspace workspace, DatabaseRecord databaseRecord)
        {
            this.Workspace = workspace;
            this.Id = databaseRecord.Id;
            this.rangeId = this.Id;
            this.Class = databaseRecord.Class;
        }

        public long Version => this.DatabaseOriginState.Version;

        public Workspace Workspace { get; }

        public DatabaseOriginState DatabaseOriginState { get; protected set; }

        IWorkspace IObject.Workspace => this.Workspace;

        public Class Class { get; }

        public long Id { get; private set; }

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

        public object GetUnitRole(RoleType roleType) => this.CanRead(roleType) ? this.DatabaseOriginState.GetUnitRole(roleType) : null;

        public IObject GetCompositeRole(RoleType roleType) =>
            this.CanRead(roleType)
                ? this.DatabaseOriginState.GetCompositeRole(roleType)
                : null;

        public IEnumerable<IObject> GetCompositesRole(RoleType roleType) =>
            this.CanRead(roleType)
                ? this.DatabaseOriginState.GetCompositesRole(roleType).Cast<IObject>()
                : Array.Empty<IObject>();

        public bool CanRead(RoleType roleType) => this.DatabaseOriginState.CanRead(roleType);

        public bool CanWrite(RoleType roleType) => this.DatabaseOriginState.CanWrite(roleType);

        public bool CanExecute(MethodType methodType) => this.DatabaseOriginState.CanExecute(methodType);

        int IComparable<Strategy>.CompareTo(Strategy other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.rangeId.CompareTo(other.rangeId);
        }
    }
}

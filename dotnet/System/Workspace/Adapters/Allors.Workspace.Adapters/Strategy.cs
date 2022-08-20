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

    public abstract class Strategy : IStrategy, IComparable<Strategy>
    {
        private readonly long rangeId;

        private IObject @object;

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

        IWorkspace IStrategy.Workspace => this.Workspace;

        public Class Class { get; }

        public long Id { get; private set; }

        public IObject Object => this.@object ??= this.Workspace.WorkspaceConnection.DatabaseConnection.Configuration.ObjectFactory.Create(this);

        public bool ExistRole(RoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType) != null;
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole<IObject>(roleType) != null;
            }

            return this.GetCompositesRole<IObject>(roleType).Any();
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
                return this.GetCompositeRole<IObject>(roleType);
            }

            return this.GetCompositesRole<IObject>(roleType);
        }

        public object GetUnitRole(RoleType roleType) => this.CanRead(roleType) ? this.DatabaseOriginState.GetUnitRole(roleType) : null;

        public T GetCompositeRole<T>(RoleType roleType) where T : class, IObject =>
            this.CanRead(roleType)
                ? (T)this.DatabaseOriginState.GetCompositeRole(roleType)?.Object
                : null;

        public IEnumerable<T> GetCompositesRole<T>(RoleType roleType) where T : class, IObject =>
            this.CanRead(roleType)
                ? this.DatabaseOriginState.GetCompositesRole(roleType).Select(v => (T)v.Object)
                : Array.Empty<T>();

        public T GetCompositeAssociation<T>(AssociationType associationType) where T : class, IObject => (T)this.Workspace.GetCompositeAssociation(this, associationType)?.Object;

        public IEnumerable<T> GetCompositesAssociation<T>(AssociationType associationType) where T : class, IObject => this.Workspace.GetCompositesAssociation(this, associationType).Select(v => v.Object).Cast<T>();

        public bool CanRead(RoleType roleType) => this.DatabaseOriginState.CanRead(roleType);

        public bool CanWrite(RoleType roleType) => this.DatabaseOriginState.CanWrite(roleType);

        public bool CanExecute(MethodType methodType) => this.DatabaseOriginState.CanExecute(methodType);

        public bool IsCompositeAssociationForRole(RoleType roleType, Strategy forRole) => this.DatabaseOriginState.IsAssociationForRole(roleType, forRole);

        public bool IsCompositesAssociationForRole(RoleType roleType, Strategy forRoleId) => this.DatabaseOriginState.IsAssociationForRole(roleType, forRoleId);

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

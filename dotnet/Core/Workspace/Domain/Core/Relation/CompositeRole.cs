// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public abstract class CompositeRole<T> : ICompositeRole where T : class, IObject
    {
        protected CompositeRole(IStrategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
            this.O = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object { get; }

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType { get; }

        private IObjectFactory O { get; set; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (T)value;
        }

        IStrategy ICompositeRole.Value
        {
            get => this.Object.GetCompositeRole(this.RoleType);
            set => this.Object.SetCompositeRole(this.RoleType, value);
        }

        public T Value
        {
            get => this.O.Object<T>(this.Object.GetCompositeRole(this.RoleType));
            set => this.Object.SetCompositeRole(this.RoleType, value?.Strategy);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);

        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }
    }
}

// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public abstract class CompositeRole<T> : ICompositeRole where T : class, IObject
    {
        private readonly ICompositeRole role;

        protected CompositeRole(IStrategy strategy, IRoleType roleType)
        {
            this.role = strategy.CompositeRole(roleType);
            this.O = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.role.Object;

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType => this.role.RoleType;

        private IObjectFactory O { get; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (T)value;
        }

        IStrategy ICompositeRole.Value
        {
            get => this.role.Value;
            set => this.role.Value = value;
        }

        public T Value
        {
            get => this.O.Object<T>(this.role.Value);
            set => this.role.Value = value?.Strategy;
        }

        public bool CanRead => this.role.CanRead;

        public bool CanWrite => this.role.CanWrite;

        public bool Exist => this.role.Exist;

        public bool IsModified => this.role.IsModified;

        public void Restore()
        {
            this.role.Restore();
        }
    }
}

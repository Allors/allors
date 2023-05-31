// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class CompositesRole<T> : ICompositesRole where T : class, IObject
    {
        public CompositesRole(IStrategy strategy, IRoleType roleType)
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
            set => this.Value = (IEnumerable<T>)value;
        }

        void ICompositesRole.Add(IStrategy strategy)
        {
            this.Object.AddCompositesRole(this.RoleType, strategy);
        }

        void ICompositesRole.Remove(IStrategy strategy)
        {
            this.Object.RemoveCompositesRole(this.RoleType, strategy);
        }

        IEnumerable<IStrategy> ICompositesRole.Value
        {
            get => this.Object.GetCompositesRole(this.RoleType);
            set => this.Object.SetCompositesRole(this.RoleType, value);
        }

        public void Add(T value)
        {
            this.Object.AddCompositesRole(this.RoleType, value.Strategy);
        }

        public void Remove(T value)
        {
            this.Object.RemoveCompositesRole(this.RoleType, value.Strategy);
        }

        public IEnumerable<T> Value
        {
            get => this.Object.GetCompositesRole(this.RoleType).Select(v => this.O.Object<T>(v));
            set => this.Object.SetCompositesRole(this.RoleType, value.Select(v => v?.Strategy));
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

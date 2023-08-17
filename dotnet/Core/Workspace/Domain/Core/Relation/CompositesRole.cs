// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Meta;

    public abstract class CompositesRole<T> : ICompositesRole<T> where T : class, IObject
    {
        private readonly ICompositesRole role;

        protected CompositesRole(IStrategy strategy, IRoleType roleType)
        {
            this.role = strategy.CompositesRole(roleType);
            this.ObjectFactory = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.role.Object;

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType => this.role.RoleType;

        private IObjectFactory ObjectFactory { get; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (IEnumerable<T>)value;
        }

        void ICompositesRole.Add(IStrategy strategy)
        {
            this.role.Add(strategy);
        }

        public void Add(T value)
        {
            this.role.Add(value?.Strategy);
        }

        void ICompositesRole.Remove(IStrategy strategy)
        {
            this.role.Remove(strategy);
        }

        public void Remove(T value)
        {
            this.role.Remove(value?.Strategy);
        }

        public IEnumerable<T> Value
        {
            get => this.ObjectFactory.Object<T>(this.role.Value);
            set => this.role.Value = value.Select(v => v?.Strategy);
        }

        public bool CanRead => this.role.CanRead;

        public bool CanWrite => this.role.CanWrite;

        public bool Exist => this.role.Exist;

        public bool IsModified => this.role.IsModified;

        IEnumerable<IStrategy> ICompositesRole.Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this.role.PropertyChanged += value;
            }

            remove
            {
                this.role.PropertyChanged -= value;
            }
        }
        public void Restore()
        {
            this.role.Restore();
        }
    }
}

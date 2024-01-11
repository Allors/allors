// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using Adapters;
    using Meta;
    using Signals;

    public class CompositesRole<T> : ICompositesRole<T>, IRoleInternal
        where T : class, IObject
    {
        private long databaseVersion;

        public CompositesRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType { get; }

        void ICompositesRole<T>.Add(T @object)
        {
            this.Add(@object?.Strategy);
        }

        void ICompositesRole<T>.Remove(T @object)
        {
            this.Remove(@object.Strategy);
        }

        IEnumerable<T> ICompositesRole<T>.Value
        {
            get => this.Value.Select(this.Object.Workspace.ObjectFactory.Object<T>);
            set => this.Value = value.Where(v => v != null).Select(v => v.Strategy);
        }

        object IRelationEnd.Value => this.Value;

        public void Add(IStrategy value)
        {
            this.Object.AddCompositesRole(this.RoleType, value);
        }

        public void Remove(IStrategy value)
        {
            this.Object.RemoveCompositesRole(this.RoleType, value);
        }

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (IEnumerable<IStrategy>)value;
        }

        public IEnumerable<IStrategy> Value
        {
            get => this.Object.GetCompositesRole(this.RoleType);
            set => this.Object.SetCompositesRole(this.RoleType, value);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);

        public long Version { get; private set; }
        
        public event ChangedEventHandler Changed
        {
            add
            {
                this.Object.Workspace.Add(this, value);
            }
            remove
            {
                this.Object.Workspace.Remove(this, value);
            }
        }

        object ISignal.Value => this;

        ICompositesRole<T> ISignal<ICompositesRole<T>>.Value => this;
        
        public void BumpVersion()
        {
            ++this.Version;
        }

        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }

        public override string ToString()
        {
            return this.Value != null ? $"[{string.Join(", ", this.Value.Select(v => v.Id))}]" : "[]";
        }
    }
}

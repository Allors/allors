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

    public class CompositesAssociation<T> : ICompositesAssociation<T>, IAssociationInternal
        where T : class, IObject
    {
        private long databaseVersion;

        public CompositesAssociation(Strategy @object, IAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }
        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }


        public IRelationType RelationType => this.AssociationType.RelationType;

        IEnumerable<T> ICompositesAssociation<T>.Value => this.Value.Select(this.Object.Workspace.ObjectFactory.Object<T>);

        public IAssociationType AssociationType { get; }

        object IRelationEnd.Value => this.Value;

        public IEnumerable<IStrategy> Value => this.Object.GetCompositesAssociation(this.AssociationType);

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

        ICompositesAssociation<T> ISignal<ICompositesAssociation<T>>.Value => this;

        public void BumpVersion()
        {
            ++this.Version;
        }

        public override string ToString()
        {
            return this.Value != null ? $"[{string.Join(", ", this.Value.Select(v => v.Id))}]" : "[]";
        }
    }
}

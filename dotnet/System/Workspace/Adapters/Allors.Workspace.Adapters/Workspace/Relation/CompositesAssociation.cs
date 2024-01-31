// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Adapters;
    using Meta;

    public class CompositesAssociation<T> : ICompositesAssociation<T>
        where T : class, IObject
    {
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

        #region Reactive
        public IDisposable Subscribe(IObserver<IObserved> observer)
        {
            return this.Object.Workspace.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<IOperand> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        public IDisposable Subscribe(IObserver<ICompositesAssociation> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        public IDisposable Subscribe(IObserver<IRelationEnd> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        public IDisposable Subscribe(IObserver<IAssociation> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        public IDisposable Subscribe(IObserver<ICompositesAssociation<T>> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        #endregion
        
        public override string ToString()
        {
            return this.Value != null ? $"[{string.Join(", ", this.Value.Select(v => v.Id))}]" : "[]";
        }
    }
}

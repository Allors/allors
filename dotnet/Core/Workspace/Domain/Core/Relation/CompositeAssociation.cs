// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.ComponentModel;
    using Meta;

    public class CompositeAssociation<T> : ICompositeAssociation<T> where T : class, IObject
    {
        private readonly ICompositeAssociation association;

        public CompositeAssociation(IStrategy strategy, IAssociationType associationType)
        {
            this.association = strategy.CompositeAssociation(associationType); 
            this.ObjectFactory = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.association.Object;

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType => this.association.AssociationType;

        private IObjectFactory ObjectFactory { get; }

        object IRelationEnd.Value => this.association.Value;

        IStrategy ICompositeAssociation.Value => this.association.Value;

        public T Value
        {
            get => this.ObjectFactory.Object<T>(this.association.Value);
        }
    }
}

// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public abstract class CompositeAssociation<T> : ICompositeAssociation where T : class, IObject
    {
        private readonly ICompositeAssociation association;

        protected CompositeAssociation(IStrategy strategy, IAssociationType associationType)
        {
            this.association = strategy.CompositeAssociation(associationType); 
            this.O = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.association.Object;

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType => this.association.AssociationType;

        private IObjectFactory O { get; }

        object IRelationEnd.Value => this.Value;

        IStrategy ICompositeAssociation.Value
        {
            get => this.association.Value;
        }

        public T Value
        {
            get => this.O.Object<T>(this.association.Value);
        }
    }
}

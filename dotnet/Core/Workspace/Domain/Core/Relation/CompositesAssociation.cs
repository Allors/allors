// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public class CompositesAssociation<T> : ICompositesAssociation<T> where T : class, IObject
    {
        private readonly ICompositesAssociation association;

        public CompositesAssociation(IStrategy strategy, IAssociationType associationType)
        {
            this.association = strategy.CompositesAssociation(associationType);
            this.ObjectFactory = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.association.Object;

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType => this.association.AssociationType;

        private IObjectFactory ObjectFactory { get; }

        object IRelationEnd.Value => this.Value;

        IEnumerable<IStrategy> ICompositesAssociation.Value => this.association.Value;

        public IEnumerable<T> Value
        {
            get => this.ObjectFactory.Object<T>(this.association.Value);
        }
    }
}

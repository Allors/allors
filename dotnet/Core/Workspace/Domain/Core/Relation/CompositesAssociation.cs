// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public abstract class CompositesAssociation<T> : ICompositesAssociation where T : class, IObject
    {
        private readonly ICompositesAssociation association;

        protected CompositesAssociation(IStrategy strategy, IAssociationType associationType)
        {
            this.association = strategy.CompositesAssociation(associationType);
            this.O = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object => this.association.Object;

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType => this.association.AssociationType;

        private IObjectFactory O { get; }

        object IRelationEnd.Value => this.Value;

        IEnumerable<IStrategy> ICompositesAssociation.Value
        {
            get => this.association.Value;
        }

        public IEnumerable<T> Value
        {
            get => this.O.Object<T>(this.association.Value);
        }
    }
}

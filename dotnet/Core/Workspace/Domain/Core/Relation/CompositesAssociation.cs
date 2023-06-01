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
        protected CompositesAssociation(IStrategy strategy, IAssociationType roleType)
        {
            this.Object = strategy;
            this.AssociationType = roleType;
            this.O = strategy.Workspace.Services.Get<IObjectFactory>();
        }

        public IStrategy Object { get; }

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType { get; }

        private IObjectFactory O { get; set; }

        object IRelationEnd.Value => this.Value;

        IEnumerable<IStrategy> ICompositesAssociation.Value
        {
            get => this.Object.GetCompositesAssociation(this.AssociationType);
        }

        public IEnumerable<T> Value
        {
            get => this.O.Object<T>(this.Object.GetCompositesAssociation(this.AssociationType));
        }
    }
}

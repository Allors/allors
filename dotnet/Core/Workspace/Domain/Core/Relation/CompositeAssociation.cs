// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public abstract class CompositeAssociation<T> : ICompositeAssociation where T : class, IObject
    {
        protected CompositeAssociation(IStrategy strategy, IAssociationType roleType)
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

        IStrategy ICompositeAssociation.Value
        {
            get => this.Object.GetCompositeAssociation(this.AssociationType);
        }

        public T Value
        {
            get => this.O.Object<T>(this.Object.GetCompositeAssociation(this.AssociationType));
        }
    }
}

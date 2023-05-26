// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;

    public class CompositesAssociation : ICompositesAssociation
    {
        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType { get; }

        public IStrategy Object { get; }

        public CompositesAssociation(Strategy @object, IAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }
    }
}

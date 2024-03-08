// <copyright file="Filter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Filter(IComposite objectType) : Extent, IPredicateContainer
    {
        public IComposite ObjectType { get; set; } = objectType;

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;

        public void Accept(IVisitor visitor) => visitor.VisitFilter(this);
    }
}

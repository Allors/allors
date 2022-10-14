// <copyright file="Filter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public class Filter : IExtent, IPredicateContainer
    {
        public Filter(IComposite objectType) => this.ObjectType = objectType;

        public IPredicate Predicate { get; set; }

        public IComposite ObjectType { get; set; }

        public Sort[] Sorting { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitFilter(this);

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;
    }
}

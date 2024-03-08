// <copyright file="Not.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    

    public class Not(IPredicate operand = null) : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public IPredicate Operand { get; set; } = operand;

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operand = predicate;

        public void Accept(IVisitor visitor) => visitor.VisitNot(this);
    }
}

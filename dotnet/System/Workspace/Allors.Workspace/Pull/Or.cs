﻿// <copyright file="Or.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;


    public class Or(params IPredicate[] operands) : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public IPredicate[] Operands { get; set; } = operands;

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operands = new List<IPredicate>(this.Operands) { predicate }.ToArray();

        public void Accept(IVisitor visitor) => visitor.VisitOr(this);
    }
}

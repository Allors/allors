﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using Adapters;
    using Meta;

    public class CompositesAssociation : ICompositesAssociation
    {
        public CompositesAssociation(Strategy @object, IAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }
        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }


        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType { get; }

        object IRelationEnd.Value => this.Value;

        public IEnumerable<IStrategy> Value => this.Object.GetCompositesAssociation(this.AssociationType);

        public override string ToString()
        {
            return this.Value != null ? $"[{string.Join(", ", this.Value.Select(v => v.Id))}]" : "[]";
        }
    }
}

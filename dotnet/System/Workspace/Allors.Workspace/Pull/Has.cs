// <copyright file="Contains.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    
    using Meta;

    public class Has(IRelationEndType propertyType = null) : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public IRelationEndType RelationEndType { get; set; } = propertyType;

        public IStrategy Object { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitHas(this);
    }
}

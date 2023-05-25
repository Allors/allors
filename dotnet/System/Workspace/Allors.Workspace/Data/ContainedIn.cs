// <copyright file="ContainedIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Workspace;
    using Meta;

    public class ContainedIn : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public ContainedIn(IRelationEndType propertyType = null) => this.PropertyType = propertyType;

        public IRelationEndType PropertyType { get; set; }

        public Extent Extent { get; set; }

        public IEnumerable<IStrategy> Objects { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitContainedIn(this);
    }
}

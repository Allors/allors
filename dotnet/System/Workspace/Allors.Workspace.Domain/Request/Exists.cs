// <copyright file="Exists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public class Exists : IPropertyPredicate
    {
        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Parameter { get; set; }

        public IPropertyType PropertyType { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitExists(this);
    }
}

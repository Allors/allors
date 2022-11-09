// <copyright file="Equals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;
    using Allors.Workspace.Request.Visitor;

    public class Equals : IPropertyPredicate
    {
        public Equals(IRelationEndType relationEndType = null) => this.RelationEndType = relationEndType;

        public IObject Object { get; set; }

        public object Value { get; set; }

        public IRoleType Path { get; set; }

        public string Parameter { get; set; }

        /// <inheritdoc />
        public IRelationEndType RelationEndType { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitEquals(this);
    }
}

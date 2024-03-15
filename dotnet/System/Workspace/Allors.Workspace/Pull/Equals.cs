// <copyright file="Equals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{

    using Meta;

    public class Equals(IRelationEndType relationEndType = null) : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        /// <inheritdoc/>
        public IRelationEndType RelationEndType { get; set; } = relationEndType;

        public IStrategy Object { get; set; }

        public long? ObjectId { get; set; }

        public object Value { get; set; }

        public IRoleType Path { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitEquals(this);
    }
}

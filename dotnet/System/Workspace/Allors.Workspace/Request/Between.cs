// <copyright file="Between.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Meta;
    using Visitor;

    public class Between : IRolePredicate
    {
        public Between(RoleType roleType = null) => this.RoleType = roleType;

        public IEnumerable<object> Values { get; set; }

        public IEnumerable<RoleType> Paths { get; set; }

        public string Parameter { get; set; }

        public RoleType RoleType { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitBetween(this);
    }
}

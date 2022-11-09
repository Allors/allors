// <copyright file="GreaterThan.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public class GreaterThan : IRolePredicate
    {
        public GreaterThan(IRoleType roleType = null) => this.RoleType = roleType;

        public object Value { get; set; }

        public IRoleType Path { get; set; }

        public string Parameter { get; set; }

        public IRoleType RoleType { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitGreaterThan(this);
    }
}

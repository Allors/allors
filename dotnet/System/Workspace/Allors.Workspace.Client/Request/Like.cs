// <copyright file="Like.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public class Like : IRolePredicate
    {
        public Like(IRoleType roleType = null) => this.RoleType = roleType;

        public string Value { get; set; }

        public string Parameter { get; set; }

        public IRoleType RoleType { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitLike(this);
    }
}

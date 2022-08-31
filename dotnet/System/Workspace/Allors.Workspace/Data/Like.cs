// <copyright file="Like.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Like : IRolePredicate
    {
        public Like(RoleType roleType = null) => this.RoleType = roleType;

        public RoleType RoleType { get; set; }

        public string Value { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitLike(this);
    }
}

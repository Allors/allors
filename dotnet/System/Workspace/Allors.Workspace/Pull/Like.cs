﻿// <copyright file="Like.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    
    using Meta;

    public class Like(IRoleType roleType = null) : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public IRoleType RoleType { get; set; } = roleType;

        public string Value { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitLike(this);
    }
}

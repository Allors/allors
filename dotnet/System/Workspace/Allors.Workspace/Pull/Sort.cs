// <copyright file="Sort.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Sort(IRoleType roleType = null) : IVisitable
    {
        public IRoleType RoleType { get; set; } = roleType;

        public SortDirection? SortDirection { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitSort(this);
    }
}

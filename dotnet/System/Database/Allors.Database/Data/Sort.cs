// <copyright file="Sort.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Sort : IVisitable
{
    public Sort(RoleType roleType = null) => this.RoleType = roleType;

    public RoleType RoleType { get; set; }

    public SortDirection SortDirection { get; set; }

    public void Accept(IVisitor visitor) => visitor.VisitSort(this);

    public void Build(Database.Extent extent) => extent.AddSort(this.RoleType, this.SortDirection);
}

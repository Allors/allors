﻿// <copyright file="ExtentStatementChild.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal class ExtentStatementChild : ExtentStatement
{
    private readonly ExtentStatementRoot root;

    internal ExtentStatementChild(ExtentStatementRoot root, Extent extent, RoleType roleType)
        : base(extent)
    {
        this.root = root;
        this.RoleType = roleType;
    }

    internal ExtentStatementChild(ExtentStatementRoot root, Extent extent, AssociationType associationType)
        : base(extent)
    {
        this.root = root;
        this.AssociationType = associationType;
    }

    internal AssociationType AssociationType { get; }

    internal override bool IsRoot => false;

    internal RoleType RoleType { get; }

    public override string ToString() => this.root.ToString();

    internal override string AddParameter(object obj) => this.root.AddParameter(obj);

    internal override void Append(string part) => this.root.Append(part);

    internal override string CreateAlias() => this.root.CreateAlias();

    internal override ExtentStatement CreateChild(Extent extent, AssociationType association) =>
        new ExtentStatementChild(this.root, extent, association);

    internal override ExtentStatement CreateChild(Extent extent, RoleType role) => new ExtentStatementChild(this.root, extent, role);
}

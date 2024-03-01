﻿// <copyright file="Equals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

internal sealed class EqualsComposite : Equals
{
    private readonly IObject obj;

    internal EqualsComposite(IObject obj)
    {
        PredicateAssertions.ValidateEquals(obj);
        this.obj = obj;
    }

    internal override void Setup(ExtentStatement statement)
    {
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        statement.Append(" (" + alias + "." + Mapping.ColumnNameForObject + "=" + statement.AddParameter(this.obj) + ") ");
        return this.Include;
    }
}

﻿// <copyright file="RoleBetweenValue.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class RoleBetweenValue : Between
{
    private readonly RoleType roleType;
    private readonly object first;
    private readonly object second;

    internal RoleBetweenValue(IInternalExtentFiltered extent, RoleType roleType, object first, object second)
    {
        extent.CheckRole(roleType);
        PredicateAssertions.ValidateRoleBetween(roleType, first, second);
        this.roleType = roleType;
        this.first = roleType.Normalize(first);
        this.second = roleType.Normalize(second);
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.roleType.RelationType] + " BETWEEN " +
                         statement.AddParameter(this.first) + " AND " + statement.AddParameter(this.second) + ")");
        return this.Include;
    }

    internal override void Setup(ExtentStatement statement) => statement.UseRole(this.roleType);
}

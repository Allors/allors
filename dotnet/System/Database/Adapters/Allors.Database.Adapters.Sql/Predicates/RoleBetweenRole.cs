// <copyright file="RoleBetweenRole.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class RoleBetweenRole : Between
{
    private readonly RoleType first;
    private readonly RoleType role;
    private readonly RoleType second;

    internal RoleBetweenRole(IInternalExtentFiltered extent, RoleType role, RoleType first, RoleType second)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleBetween(role, first, second);
        this.role = role;
        this.first = first;
        this.second = second;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " BETWEEN " + alias + "." +
                         schema.ColumnNameByRelationType[this.first.RelationType] + " AND " + alias + "." +
                         schema.ColumnNameByRelationType[this.second.RelationType] + ")");
        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        statement.UseRole(this.role);
        statement.UseRole(this.first);
        statement.UseRole(this.second);
    }
}

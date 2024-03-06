// <copyright file="RoleLessThanRole.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class RoleLessThanRole : LessThan
{
    private readonly RoleType lessThanRole;
    private readonly RoleType role;

    internal RoleLessThanRole(IInternalExtentFiltered extent, RoleType role, RoleType lessThanRole)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleLessThan(role, lessThanRole);
        this.role = role;
        this.lessThanRole = lessThanRole;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " < " + alias + "." +
                         schema.ColumnNameByRelationType[this.lessThanRole.RelationType]);
        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        statement.UseRole(this.role);
        statement.UseRole(this.lessThanRole);
    }
}

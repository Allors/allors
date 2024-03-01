﻿// <copyright file="RoleGreaterThanRole.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class RoleGreaterThanRole : GreaterThan
{
    private readonly RoleType greaterThanRole;
    private readonly RoleType role;

    internal RoleGreaterThanRole(ExtentFiltered extent, RoleType role, RoleType greaterThanRole)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleGreaterThan(role, greaterThanRole);
        this.role = role;
        this.greaterThanRole = greaterThanRole;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + ">" + alias + "." +
                         schema.ColumnNameByRelationType[this.greaterThanRole.RelationType]);
        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        statement.UseRole(this.role);
        statement.UseRole(this.greaterThanRole);
    }
}

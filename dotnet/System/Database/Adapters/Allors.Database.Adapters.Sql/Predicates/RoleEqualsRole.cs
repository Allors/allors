// <copyright file="RoleEqualsRole.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsPredicateRoleEqualsRoleSql type.
// </summary>

namespace Allors.Database.Adapters.Sql;

using System;
using Allors.Database.Meta;

internal sealed class RoleEqualsRole : Equals
{
    private readonly RoleType equalsRole;
    private readonly RoleType role;

    internal RoleEqualsRole(IInternalExtentFiltered extent, RoleType role, RoleType equalsRole)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleEquals(role, equalsRole);
        this.role = role;
        this.equalsRole = equalsRole;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        if (this.role.ObjectType.IsUnit && this.equalsRole.ObjectType.IsUnit)
        {
            if (this.role.ObjectType.Tag == UnitTags.String)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRoleType[this.role] +
                                 $" {schema.StringCollation} =" + alias + "." +
                                 schema.ColumnNameByRoleType[this.equalsRole] + $" {schema.StringCollation}");
            }
            else
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRoleType[this.role] + "=" + alias + "." +
                                 schema.ColumnNameByRoleType[this.equalsRole]);
            }
        }
        else if (((Composite)this.role.ObjectType).ExclusiveClass != null && ((Composite)this.equalsRole.ObjectType).ExclusiveClass != null)
        {
            statement.Append(" " + alias + "." + schema.ColumnNameByRoleType[this.role] + "=" + alias + "." +
                             schema.ColumnNameByRoleType[this.equalsRole]);
        }
        else
        {
            throw new NotImplementedException();
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        statement.UseRole(this.role);
        statement.UseRole(this.equalsRole);
    }
}

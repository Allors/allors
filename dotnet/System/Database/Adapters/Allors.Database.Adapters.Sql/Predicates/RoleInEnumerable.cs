// <copyright file="RoleInEnumerable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System.Collections.Generic;
using System.Text;
using Allors.Database.Meta;

internal sealed class RoleInEnumerable : In
{
    private readonly IEnumerable<IObject> enumerable;
    private readonly RoleType role;

    internal RoleInEnumerable(IInternalExtentFiltered extent, RoleType role, IEnumerable<IObject> enumerable)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleIn(role, enumerable);
        this.role = role;
        this.enumerable = enumerable;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;

        var inStatement = new StringBuilder("0");
        foreach (var inObject in this.enumerable)
        {
            inStatement.Append(",");
            inStatement.Append(inObject.Id);
        }

        if (!this.role.ExistExclusiveClasses)
        {
            // TODO: in combination with NOT gives error
            statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IN (");
            statement.Append(inStatement.ToString());
            statement.Append(" ))");
        }
        else if (this.role.IsMany)
        {
            statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IN (");
            statement.Append(inStatement.ToString());
            statement.Append(" ))");
        }
        else
        {
            statement.Append(" (" + schema.ColumnNameByRoleType[this.role] + " IS NOT NULL AND ");
            statement.Append(" " + schema.ColumnNameByRoleType[this.role] + " IN (");
            statement.Append(inStatement.ToString());
            statement.Append(" ))");
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
}

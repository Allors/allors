// <copyright file="RoleContains.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsPredicateRoleContainsSql type.
// </summary>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class RoleHas : Has
{
    private readonly IObject allorsObject;
    private readonly RoleType role;

    internal RoleHas(IInternalExtentFiltered extent, RoleType role, IObject allorsObject)
    {
        extent.CheckRole(role);
        PredicateAssertions.ValidateRoleContains(role, allorsObject);
        this.role = role;
        this.allorsObject = allorsObject;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        if ((this.role.IsMany && this.role.AssociationType.IsMany) || !this.role.ExistExclusiveClasses)
        {
            statement.Append("\n");
            statement.Append("EXISTS(\n");
            statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject + "\n");
            statement.Append("FROM " + schema.TableNameForRelationByRoleType[this.role] + "\n");
            statement.Append("WHERE   " + Mapping.ColumnNameForAssociation + "=" + alias + "." + Mapping.ColumnNameForObject + "\n");
            statement.Append("AND " + Mapping.ColumnNameForRole + "=" + this.allorsObject.Strategy.ObjectId + "\n");
            statement.Append(")\n");
        }
        else
        {
            // This join is not possible because of the 3VL (Three Valued Logic).
            // It should work for normal queries, but it fails when wrapped in a NOT( ... ) predicate.
            // The rows with NULL values should then return TRUE and not UNKNOWN.
            //
            // statement.sql.Append(" " + role.SingularFullName + "_R." + schema.O + " = " + allorsObject.ObjectId);
            statement.Append("\n");
            statement.Append("EXISTS(\n");
            statement.Append("SELECT " + Mapping.ColumnNameForObject + "\n");
            statement.Append("FROM " + schema.TableNameForObjectByClass[((Composite)this.role.ObjectType).ExclusiveClass] + "\n");
            statement.Append("WHERE " + Mapping.ColumnNameForObject + "=" + this.allorsObject.Strategy.ObjectId + "\n");
            statement.Append("AND " + schema.ColumnNameByRoleType[this.role] + "=" + alias + ".O\n");
            statement.Append(")\n");
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        // extent.UseRole(role);
    }
}

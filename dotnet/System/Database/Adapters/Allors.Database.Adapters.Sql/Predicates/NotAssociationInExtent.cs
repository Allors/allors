// <copyright file="NotAssociationInExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class NotAssociationInExtent : In
{
    private readonly AssociationType association;
    private readonly IInternalExtent inExtent;

    internal NotAssociationInExtent(IInternalExtentFiltered extent, AssociationType association, Allors.Database.IExtent<IObject> inExtent)
    {
        extent.CheckAssociation(association);
        PredicateAssertions.AssertAssociationIn(association, inExtent);
        this.association = association;
        this.inExtent = ((IInternalExtent)inExtent).InExtent;
    }

    internal override bool IsNotFilter => true;

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        var inStatement = statement.CreateChild(this.inExtent, this.association);

        inStatement.UseRole(this.association.RoleType);

        if (!this.association.RoleType.ExistExclusiveClasses)
        {
            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IS NULL OR");
            statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IN (\n");
            statement.Append(" SELECT " + Mapping.ColumnNameForRole + " FROM " +
                             schema.TableNameForRelationByRoleType[this.association.RoleType] + " WHERE " +
                             Mapping.ColumnNameForAssociation + " IN (");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" )))\n");
        }
        else if (this.association.RoleType.IsMany)
        {
            statement.Append(" (" + alias + "." + schema.ColumnNameByRoleType[this.association.RoleType] + " IS NULL OR ");
            statement.Append(" NOT " + alias + "." + schema.ColumnNameByRoleType[this.association.RoleType] + " IN (\n");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
        }
        else
        {
            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NULL OR ");
            statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IN (\n");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
}

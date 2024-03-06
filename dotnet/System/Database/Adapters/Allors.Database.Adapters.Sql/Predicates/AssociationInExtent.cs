// <copyright file="AssociationInExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class AssociationInExtent : In
{
    private readonly AssociationType association;
    private readonly IInternalExtent inExtent;

    internal AssociationInExtent(IInternalExtentFiltered extent, AssociationType association, Allors.Database.IExtent<IObject> inExtent)
    {
        extent.CheckAssociation(association);
        PredicateAssertions.AssertAssociationIn(association, inExtent);
        this.association = association;
        this.inExtent = ((IInternalExtent)inExtent).InExtent;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;
        var inStatement = statement.CreateChild(this.inExtent, this.association);

        inStatement.UseRole(this.association.RoleType);

        if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveClasses)
        {
            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL AND ");
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IN (\n");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
        }
        else if (this.association.RoleType.IsMany)
        {
            statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NOT NULL AND ");
            statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IN (\n");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
        }
        else
        {
            statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IN (\n");
            this.inExtent.BuildSql(inStatement);
            statement.Append(" ))\n");
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
}

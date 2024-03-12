// <copyright file="AssociationExists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

internal sealed class AssociationExists : Exists
{
    private readonly AssociationType association;

    internal AssociationExists(IInternalExtentFiltered extent, AssociationType association)
    {
        extent.CheckAssociation(association);
        PredicateAssertions.ValidateAssociationExists(association);
        this.association = association;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        var schema = statement.Mapping;

        if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RoleType.ExistExclusiveClasses)
        {
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL");
        }
        else if (this.association.RoleType.IsMany)
        {
            statement.Append(" " + alias + "." + schema.ColumnNameByRoleType[this.association.RoleType] + " IS NOT NULL");
        }
        else
        {
            statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL");
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
}

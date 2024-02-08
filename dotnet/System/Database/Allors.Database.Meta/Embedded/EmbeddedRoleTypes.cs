// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using Embedded.Meta;

public class EmbeddedRoleTypes
{
    public EmbeddedRoleTypes(EmbeddedMeta meta)
    {
        this.ObjectTypeSingularName = meta.AddUnit<IObjectType, string>(nameof(IObjectType.SingularName));
        this.ObjectTypeAssignedPluralName = meta.AddUnit<IObjectType, string>(nameof(IObjectType.AssignedPluralName));
        this.ObjectTypePluralName = meta.AddUnit<IObjectType, string>(nameof(IObjectType.PluralName));
    }

    public EmbeddedRoleType ObjectTypeSingularName { get; set; }

    public EmbeddedRoleType ObjectTypeAssignedPluralName { get; set; }

    public EmbeddedRoleType ObjectTypePluralName { get; set; }
}

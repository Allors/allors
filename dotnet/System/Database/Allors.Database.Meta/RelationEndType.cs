// <copyright file="IRelationEndType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using Embedded.Meta;

/// <summary>
///     A <see cref="RelationEndType" /> can be a <see cref="AssociationType" /> or a <see cref="RoleType" />.
/// </summary>
public abstract class RelationEndType : OperandType
{
    protected RelationEndType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
    }

    public abstract ObjectType ObjectType { get; set; }

    public abstract string Name { get; }

    public abstract string SingularName { get; set; }

    public abstract string SingularFullName { get; }

    public abstract string PluralName { get; }

    public abstract string PluralFullName { get; }

    public abstract bool IsOne { get; }

    public abstract bool IsMany { get; }

    public static implicit operator RelationEndType(RelationEndTypeIndex index) => index?.RelationEndType;

    public static implicit operator RelationEndType(AssociationTypeIndex index) => index?.AssociationType;

    public static implicit operator RelationEndType(RoleTypeIndex index) => index?.RoleType;

    public abstract void Validate(ValidationLog validationLog);
}

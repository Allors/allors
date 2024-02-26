// <copyright file="AssociationType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta;

using System;
using Embedded.Meta;

/// <summary>
///     An <see cref="AssociationType" /> defines the association side of a relation.
///     This is also called the 'active', 'controlling' or 'owning' side.
///     AssociationTypes can only have composite <see cref="ObjectType" />s.
/// </summary>
public sealed class AssociationType : RelationEndType, IComparable
{
    private Composite objectType;
    private RelationType relationType;

    /// <summary>
    ///     Used to create property names.
    /// </summary>
    private const string Where = "Where";

    public AssociationType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
    }

    public override ObjectType ObjectType
    {
        get => this.Composite;
        set => this.Composite = (Composite)value;
    }

    public Composite Composite
    {
        get { return this.objectType; }
        set { this.objectType = value; }
    }

    public RoleType RoleType => this.relationType.RoleType;

    public RelationType RelationType
    {
        get => this.relationType;
        set => this.relationType = value;
    }

    public override string Name => this.IsMany ? this.PluralName : this.SingularName;

    public override string SingularFullName => this.SingularName;

    public override string SingularName
    {
        get => this.objectType.SingularName + Where + this.relationType.RoleType.SingularName;
        set => throw new NotSupportedException();
    }

    public override string PluralFullName => this.PluralName;

    public override string PluralName => this.objectType.PluralName + Where + this.relationType.RoleType.SingularName;

    public override bool IsOne => !this.IsMany;
    private string ValidationName => "association type " + this.Name;

    public override bool IsMany =>
        this.relationType.Multiplicity switch
        {
            Multiplicity.ManyToOne => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };

    public static implicit operator AssociationType(AssociationTypeIndex index) => index?.AssociationType;

    public int CompareTo(object other) => this.relationType.Id.CompareTo((other as AssociationType)?.relationType.Id);

    public override string ToString() => $"{this.relationType.RoleType.ObjectType.SingularName}.{this.Name}";

    public override void Validate(ValidationLog validationLog)
    {
        if (this.objectType == null)
        {
            var message = this.ValidationName + " has no object type";
            validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.IObjectType");
        }

        if (this.relationType == null)
        {
            var message = this.ValidationName + " has no relation type";
            validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.RelationType");
        }
    }
}

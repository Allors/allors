// <copyright file="AssociationType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta;

using System;

/// <summary>
///     An <see cref="AssociationType" /> defines the association side of a relation.
///     This is also called the 'active', 'controlling' or 'owning' side.
///     AssociationTypes can only have composite <see cref="ObjectType" />s.
/// </summary>
public abstract class AssociationType : IAssociationType, IComparable
{
    /// <summary>
    ///     Used to create property names.
    /// </summary>
    private const string Where = "Where";

    protected AssociationType(IComposite objectType)
    {
        this.Attributes = new MetaExtension();
        this.ObjectType = objectType;
    }

    public dynamic Attributes { get; }

    internal IComposite ObjectType { get; }

    internal RoleType RoleType => this.RelationType.RoleType;

    internal RelationType RelationType { get; set; }
    
    string IRelationEndType.Name => this.Name;

    string IRelationEndType.SingularName => this.SingularName;

    string IRelationEndType.SingularFullName => this.SingularName;

    string IRelationEndType.PluralName => this.ObjectType.PluralName + Where + this.RoleType.SingularName;

    string IRelationEndType.PluralFullName => this.PluralName;

    IObjectType IRelationEndType.ObjectType => this.ObjectType;

    IComposite IAssociationType.ObjectType => this.ObjectType;

    bool IRelationEndType.IsOne => !this.IsMany;

    bool IRelationEndType.IsMany =>
        this.RelationType.Multiplicity switch
        {
            Multiplicity.ManyToOne => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };

    IRoleType IAssociationType.RoleType => this.RoleType;

    IRelationType IAssociationType.RelationType => this.RelationType;

    private string Name => this.IsMany ? this.PluralName : this.SingularName;

    private string SingularName => this.ObjectType.SingularName + Where + this.RoleType.SingularName;

    private string PluralName => this.ObjectType.PluralName + Where + this.RoleType.SingularName;

    private string ValidationName => "association type " + this.Name;

    private bool IsMany =>
        this.RelationType.Multiplicity switch
        {
            Multiplicity.ManyToOne => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };

    public int CompareTo(object other) => this.RelationType.Id.CompareTo((other as AssociationType)?.RelationType.Id);

    public override bool Equals(object other) => this.RelationType.Id.Equals((other as AssociationType)?.RelationType.Id);

    public override int GetHashCode() => this.RelationType.Id.GetHashCode();

    public override string ToString() => $"{this.RoleType.ObjectType.Name}.{this.Name}";

    internal void Validate(ValidationLog validationLog)
    {
        if (this.ObjectType == null)
        {
            var message = this.ValidationName + " has no object type";
            validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.IObjectType");
        }

        if (this.RelationType == null)
        {
            var message = this.ValidationName + " has no relation type";
            validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.RelationType");
        }
    }
}

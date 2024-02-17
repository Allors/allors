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
public abstract class AssociationType : IComparable, IAssociationType, IRelationEndType
{
    private readonly IComposite objectType;
    private IRelationType relationType;

    /// <summary>
    ///     Used to create property names.
    /// </summary>
    private const string Where = "Where";

    protected AssociationType(IComposite objectType)
    {
        this.Attributes = new MetaExtension();
        this.objectType = objectType;
    }

    public dynamic Attributes { get; }

    IObjectType IRelationEndType.ObjectType => this.objectType;

    public IComposite ObjectType => this.objectType;
    
    public IRoleType RoleType => this.relationType.RoleType;


    public IRelationType RelationType
    {
        get => this.relationType;
        set => this.relationType = value;
    }

    string IRelationEndType.Name => this.Name;

    private string Name => this.IsMany ? this.PluralName : this.SingularName;

    string IRelationEndType.SingularFullName => this.SingularName;

    public string SingularName => this.objectType.SingularName + Where + this.relationType.RoleType.SingularName;

    string IRelationEndType.PluralName => this.objectType.PluralName + Where + this.relationType.RoleType.SingularName;

    string IRelationEndType.PluralFullName => this.PluralName;

    private string PluralName => this.objectType.PluralName + Where + this.relationType.RoleType.SingularName;
    
    bool IRelationEndType.IsOne => !this.IsMany;

    bool IRelationEndType.IsMany =>
        this.relationType.Multiplicity switch
        {
            Multiplicity.ManyToOne => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };


    private string ValidationName => "association type " + this.Name;

    private bool IsMany =>
        this.relationType.Multiplicity switch
        {
            Multiplicity.ManyToOne => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };

    public int CompareTo(object other) => this.relationType.Id.CompareTo((other as AssociationType)?.relationType.Id);

    public override bool Equals(object other) => this.relationType.Id.Equals((other as AssociationType)?.relationType.Id);

    public override int GetHashCode() => this.relationType.Id.GetHashCode();

    public override string ToString() => $"{this.relationType.RoleType.ObjectType.Name}.{this.Name}";

    public void Validate(ValidationLog validationLog)
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

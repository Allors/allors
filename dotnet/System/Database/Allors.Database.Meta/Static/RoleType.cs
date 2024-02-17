// <copyright file="RoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Graph;

public abstract class RoleType : IStaticRoleType, IComparable
{
    private readonly IObjectType objectType;

    private IStaticRelationType relationType;
    private string singularName;
    private string pluralName;
    private ICompositeRoleType compositeRoleType;

    /// <summary>
    ///     The maximum size value.
    /// </summary>
    public const int MaximumSize = -1;

    protected RoleType(IObjectType objectType, string assignedSingularName, string assignedPluralName)
    {
        this.Attributes = new MetaExtension();
        this.objectType = objectType;
        this.AssignedSingularName = !string.IsNullOrEmpty(assignedSingularName) ? assignedSingularName : null;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
    }

    public dynamic Attributes { get; }

    public IRelationType RelationType => this.relationType;

    IStaticRelationType IStaticRoleType.RelationType
    {
        get => this.relationType;
        set => this.relationType = value;
    }

    public ICompositeRoleType CompositeRoleType
    {
        get => this.compositeRoleType;
    }

    ICompositeRoleType IStaticRoleType.CompositeRoleType
    {
        get => this.compositeRoleType;
        set => this.compositeRoleType = value;
    }

    public IReadOnlyDictionary<IComposite, ICompositeRoleType> CompositeRoleTypeByComposite { get; private set; }

    IAssociationType IRoleType.AssociationType => this.relationType.AssociationType;

    IStaticAssociationType IStaticRoleType.AssociationType => this.relationType.AssociationType;

    public string AssignedSingularName { get; }

    public string AssignedPluralName { get; }

    private string ValidationName => "RoleType: " + this.relationType.Name;

    IObjectType IRelationEndType.ObjectType => this.objectType;

    string IRelationEndType.SingularName
    {
        get => this.singularName;
    }

    string IStaticRoleType.SingularName
    {
        get => this.singularName;
        set => this.singularName = value;
    }

    /// <summary>
    ///     Gets the full singular name.
    /// </summary>
    /// <value>The full singular name.</value>
    public string SingularFullName => this.relationType.AssociationType.ObjectType + this.singularName;

    string IRelationEndType.PluralName
    {
        get => this.pluralName;
    }

    string IStaticRoleType.PluralName
    {
        get => this.pluralName;
        set => this.pluralName = value;
    }

    /// <summary>
    ///     Gets the full plural name.
    /// </summary>
    /// <value>The full plural name.</value>
    public string PluralFullName => this.relationType.AssociationType.ObjectType + this.pluralName;

    public string Name => this.IsMany ? this.pluralName : this.singularName;

    public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

    public bool IsMany =>
        this.relationType.Multiplicity switch
        {
            Multiplicity.OneToMany => true,
            Multiplicity.ManyToMany => true,
            _ => false,
        };

    /// <summary>
    ///     Gets a value indicating whether this state has a multiplicity of one.
    /// </summary>
    /// <value><c>true</c> if this state is one; otherwise, <c>false</c>.</value>
    public bool IsOne => !this.IsMany;

    public int? Size { get; set; }

    public int? Precision { get; set; }

    public int? Scale { get; set; }

    public int CompareTo(object other) => this.relationType.Id.CompareTo((other as RoleType)?.relationType.Id);

    public override bool Equals(object other) => this.relationType.Id.Equals((other as RoleType)?.relationType.Id);

    public override int GetHashCode() => this.relationType.Id.GetHashCode();

    public override string ToString() => $"{this.relationType.AssociationType.ObjectType.Name}.{this.Name}";

    /// <summary>
    ///     Derive multiplicity, scale and size.
    /// </summary>
    void IStaticRoleType.DeriveScaleAndSize()
    {
        if (this.objectType is IUnit unitType)
        {
            switch (unitType.Tag)
            {
            case UnitTags.String:
                this.Size ??= 256;
                this.Scale = null;
                this.Precision = null;
                break;

            case UnitTags.Binary:
                this.Size ??= MaximumSize;
                this.Scale = null;
                this.Precision = null;
                break;

            case UnitTags.Decimal:
                this.Precision ??= 19;
                this.Scale ??= 2;
                this.Size = null;
                break;

            default:
                this.Size = null;
                this.Scale = null;
                this.Precision = null;
                break;
            }
        }
        else
        {
            this.Size = null;
            this.Scale = null;
            this.Precision = null;
        }
    }

    /// <summary>
    ///     Validates the state.
    /// </summary>
    /// <param name="validationLog">The validation.</param>
    public void Validate(ValidationLog validationLog)
    {
        if (this.objectType == null)
        {
            var message = this.ValidationName + " has no IObjectType";
            validationLog.AddError(message, this, ValidationKind.Required, "RoleType.IObjectType");
        }

        if (!string.IsNullOrEmpty(this.singularName) && this.singularName.Length < 2)
        {
            var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
            validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.SingularName");
        }

        if (!string.IsNullOrEmpty(this.pluralName) && this.pluralName.Length < 2)
        {
            var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
            validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.PluralName");
        }
    }

    void IStaticRoleType.InitializeCompositeRoleTypes(Dictionary<IComposite, HashSet<ICompositeRoleType>> compositeRoleTypesByComposite)
    {
        var composite = this.relationType.AssociationType.ObjectType;
        compositeRoleTypesByComposite[composite].Add(this.compositeRoleType);

        var dictionary = composite.Subtypes.ToDictionary(v => v, v =>
        {
            var compositeRoleType = (ICompositeRoleType)new CompositeRoleType(v, this);
            compositeRoleTypesByComposite[v].Add(compositeRoleType);
            return compositeRoleType;
        });

        dictionary[composite] = this.compositeRoleType;

        this.CompositeRoleTypeByComposite = dictionary;
    }

    void IStaticRoleType.DeriveIsRequired()
    {
        var composites = new Graph<IComposite>(this.relationType.AssociationType.ObjectType.Composites, v => v.DirectSubtypes).Reverse();

        bool previousRequired = false;
        foreach (var composite in composites)
        {
            var compositeRoleType = this.CompositeRoleTypeByComposite[composite];
            var attributes = compositeRoleType.Attributes;
            bool? assignedIsRequired = attributes.AssignedIsRequired;

            var required = previousRequired || assignedIsRequired is true;
            attributes.IsRequired = required;
            previousRequired = required;
        }
    }

    void IStaticRoleType.DeriveIsUnique()
    {
        var composites = new Graph<IComposite>(this.relationType.AssociationType.ObjectType.Composites, v => v.DirectSubtypes).Reverse();

        bool previousUnique = false;
        foreach (var composite in composites)
        {
            var compositeRoleType = this.CompositeRoleTypeByComposite[composite];
            var attributes = compositeRoleType.Attributes;
            bool? assignedIsUnique = attributes.AssignedIsUnique;

            var required = previousUnique || assignedIsUnique is true;
            attributes.IsUnique = required;
            previousUnique = required;
        }
    }
}

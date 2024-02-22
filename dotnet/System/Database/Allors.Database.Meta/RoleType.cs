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
using Text;

public sealed class RoleType : RelationEndType, IComparable
{
    private readonly IObjectType objectType;

    private RelationType relationType;
    private string singularName;
    private CompositeRoleType compositeRoleType;

    /// <summary>
    ///     The maximum size value.
    /// </summary>
    public const int MaximumSize = -1;

    public RoleType(IObjectType objectType, string assignedSingularName, string assignedPluralName)
    {
        this.objectType = objectType;
        this.AssignedSingularName = !string.IsNullOrEmpty(assignedSingularName) ? assignedSingularName : null;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
    }

    public RelationType RelationType
    {
        get => this.relationType;
        set => this.relationType = value;
    }

    public CompositeRoleType CompositeRoleType
    {
        get => this.compositeRoleType;
        set => this.compositeRoleType = value;
    }

    public IReadOnlyDictionary<Composite, CompositeRoleType> CompositeRoleTypeByComposite { get; private set; }

    public AssociationType AssociationType => this.relationType.AssociationType;

    public string AssignedSingularName { get; }

    public string AssignedPluralName { get; }

    private string ValidationName => "RoleType: " + this.relationType.Name;

    public override IObjectType ObjectType => this.objectType;

    public override string SingularName
    {
        get => this.singularName;
        set => this.singularName = value;
    }

    /// <summary>
    ///     Gets the full singular name.
    /// </summary>
    /// <value>The full singular name.</value>
    public override string SingularFullName => this.relationType.AssociationType.ObjectType + this.SingularName;

    public override string PluralName
    {
        get => this.AssignedPluralName ?? (this.AssignedSingularName != null
            ? Pluralizer.Pluralize(this.AssignedSingularName)
            : ((RelationEndType)this).ObjectType.PluralName);
    }

    /// <summary>
    ///     Gets the full plural name.
    /// </summary>
    /// <value>The full plural name.</value>
    public override string PluralFullName => this.relationType.AssociationType.ObjectType + this.PluralName;

    public override string Name => this.IsMany ? this.PluralName : this.SingularName;

    public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

    public override bool IsMany =>
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
    public override bool IsOne => !this.IsMany;

    public int? Size { get; set; }

    public int? Precision { get; set; }

    public int? Scale { get; set; }

    public static implicit operator RoleType(IRoleTypeIndex index) => index.RoleType;

    public int CompareTo(object other) => this.relationType.Id.CompareTo((other as RoleType)?.relationType.Id);

    public override bool Equals(object other) => this.relationType.Id.Equals((other as RoleType)?.relationType.Id);

    public override int GetHashCode() => this.relationType.Id.GetHashCode();

    public override string ToString() => $"{this.relationType.AssociationType.ObjectType.SingularName}.{this.Name}";

    /// <summary>
    ///     Derive multiplicity, scale and size.
    /// </summary>
    public void DeriveScaleAndSize()
    {
        if (this.objectType is Unit unitType)
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
    public override void Validate(ValidationLog validationLog)
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

        if (!string.IsNullOrEmpty(this.PluralName) && this.PluralName.Length < 2)
        {
            var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
            validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.PluralName");
        }
    }

    public void InitializeCompositeRoleTypes(Dictionary<Composite, HashSet<CompositeRoleType>> compositeRoleTypesByComposite)
    {
        var composite = this.relationType.AssociationType.ObjectTypeAsComposite;
        compositeRoleTypesByComposite[composite].Add(this.compositeRoleType);

        var dictionary = composite.Subtypes.ToDictionary(v => v, v =>
        {
            var compositeRoleType = (CompositeRoleType)new CompositeRoleType(v, this);
            compositeRoleTypesByComposite[v].Add(compositeRoleType);
            return compositeRoleType;
        });

        dictionary[composite] = this.compositeRoleType;

        this.CompositeRoleTypeByComposite = dictionary;
    }

    public void DeriveIsRequired()
    {
        var composites = new Graph<Composite>(this.relationType.AssociationType.ObjectTypeAsComposite.Composites, v => v.DirectSubtypes).Reverse();

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

    public void DeriveIsUnique()
    {
        var composites = new Graph<Composite>(this.relationType.AssociationType.ObjectTypeAsComposite.Composites, v => v.DirectSubtypes).Reverse();

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

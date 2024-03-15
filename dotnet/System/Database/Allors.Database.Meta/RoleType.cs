// <copyright file="RoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Embedded;
using Allors.Embedded.Meta;
using Allors.Graph;
using Text;

public sealed class RoleType : RelationEndType, IComparable
{
    private AssociationType associationType;
    private CompositeRoleType compositeRoleType;

    private string[] derivedWorkspaceNames;

    private readonly IEmbeddedCompositeRole<ObjectType> objectType;
    private readonly IEmbeddedUnitRole<bool> isDerived;

    /// <summary>
    ///     The maximum size value.
    /// </summary>
    public const int MaximumSize = -1;

    public RoleType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.objectType = this.EmbeddedPopulation.EmbeddedGetCompositeRole<ObjectType>(this, metaPopulation.MetaMeta.RoleTypeObjectType);
        this.isDerived = this.EmbeddedPopulation.EmbeddedGetUnitRole<bool>(this, metaPopulation.MetaMeta.RoleTypeIsDerived);

        this.AssignedWorkspaceNames = Array.Empty<string>();

        this.MetaPopulation.OnCreated(this);
    }

    public override ObjectType ObjectType
    {
        get => this.objectType.Value;
        set => this.objectType.Value = value;
    }

    public bool IsDerived
    {
        get => this.isDerived.Value; 
        set => this.isDerived.Value = value;
    }

    public bool IsRequired { get; set; }

    public bool IsUnique { get; set; }

    public string MediaType { get; set; }

    public bool IsIndexed { get; set; }

    public AssociationType AssociationType
    {
        get => this.associationType;
        set => this.associationType = value;
    }

    public CompositeRoleType CompositeRoleType
    {
        get => this.compositeRoleType;
        set => this.compositeRoleType = value;
    }

    public IReadOnlyDictionary<Composite, CompositeRoleType> CompositeRoleTypeByComposite { get; private set; }

    public string AssignedSingularName { get; set; }

    public string AssignedPluralName { get; set; }

    private string ValidationName => "RoleType: " + this.Name;

    public Multiplicity Multiplicity => this.ObjectType.IsUnit ? Multiplicity.OneToOne : this.AssignedMultiplicity ?? Multiplicity.ManyToOne;

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            return this.derivedWorkspaceNames;
        }
    }

    public IReadOnlyList<string> AssignedWorkspaceNames { get; set; }

    public bool IsKey { get; set; }
    
    public bool ExistExclusiveClasses
    {
        get
        {
            if (this.associationType?.ObjectType != null && this.ObjectType != null)
            {
                return this.associationType.Composite.ExclusiveClass != null && this.ObjectType is Composite roleCompositeType && roleCompositeType.ExclusiveClass != null;
            }

            return false;
        }
    }

    public override string SingularName
    {
        get => this.AssignedSingularName ?? this.ObjectType.SingularName;
    }

    public Multiplicity? AssignedMultiplicity { get; set; }

    /// <summary>
    ///     Gets the full singular name.
    /// </summary>
    /// <value>The full singular name.</value>
    public override string SingularFullName => this.AssociationType.ObjectType + this.SingularName;

    public override string PluralName
    {
        get => this.AssignedPluralName ?? (this.AssignedSingularName != null
            ? Pluralizer.Pluralize(this.AssignedSingularName)
            : this.ObjectType.PluralName);
    }

    /// <summary>
    ///     Gets the full plural name.
    /// </summary>
    /// <value>The full plural name.</value>
    public override string PluralFullName => this.AssociationType.ObjectType + this.PluralName;

    public override string Name => this.IsMany ? this.PluralName : this.SingularName;

    public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

    public override bool IsMany =>
        this.Multiplicity switch
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

    private string ReverseName => this.SingularName + this.associationType.ObjectType;

    public int CompareTo(object other) => this.Id.CompareTo((other as RoleType)?.Id);

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not RoleType other)
        {
            return false;
        }

        if (this.EmbeddedPopulation != other.EmbeddedPopulation)
        {
            throw new ArgumentException("Object is from another meta population");
        }

        return this == other;
    }

    /// <summary>
    ///     Derive multiplicity, scale and size.
    /// </summary>
    public void DeriveScaleAndSize()
    {
        if (this.ObjectType is Unit unitType)
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
        if (this.associationType == null)
        {
            var message = this.ValidationName + " has no association type";
            validationLog.AddError(message, this, ValidationKind.Required, "RelationType.AssociationType");
        }
        else
        {
            if (validationLog.ExistRelationName(this.FullName))
            {
                var message = "name of " + this.ValidationName + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
            }
            else
            {
                validationLog.AddRelationTypeName(this.FullName);
            }

            if (validationLog.ExistRelationName(this.ReverseName))
            {
                var message = "reversed name of " + this.ValidationName + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
            }
            else
            {
                validationLog.AddRelationTypeName(this.ReverseName);
            }

            if (validationLog.ExistObjectTypeName(this.FullName))
            {
                var message = "name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
            }

            if (validationLog.ExistObjectTypeName(this.ReverseName))
            {
                var message = "reversed name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
            }
        }

        if (this.objectType == null)
        {
            var message = this.ValidationName + " has no IObjectType";
            validationLog.AddError(message, this, ValidationKind.Required, "RoleType.IObjectType");
        }

        if (!string.IsNullOrEmpty(this.SingularName) && this.SingularName.Length < 2)
        {
            var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
            validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.SingularName");
        }

        if (!string.IsNullOrEmpty(this.PluralName) && this.PluralName.Length < 2)
        {
            var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
            validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.PluralName");
        }

        this.associationType?.Validate(validationLog);
    }

    public void InitializeCompositeRoleTypes(Dictionary<Composite, HashSet<CompositeRoleType>> compositeRoleTypesByComposite)
    {
        var composite = this.AssociationType.Composite;
        compositeRoleTypesByComposite[composite].Add(this.compositeRoleType);

        var dictionary = composite.Subtypes.ToDictionary(v => v, v =>
        {
            var compositeRoleType = new CompositeRoleType(v, this);
            compositeRoleTypesByComposite[v].Add(compositeRoleType);
            return compositeRoleType;
        });

        dictionary[composite] = this.compositeRoleType;

        this.CompositeRoleTypeByComposite = dictionary;
    }

    public void DeriveIsRequired()
    {
        var composites = new Graph<Composite>(this.AssociationType.Composite.Composites, v => v.DirectSubtypes).Reverse();

        bool previousRequired = false;
        foreach (var composite in composites)
        {
            var compositeRoleType = this.CompositeRoleTypeByComposite[composite];
            bool? assignedIsRequired = compositeRoleType.AssignedIsRequired;

            var required = previousRequired || assignedIsRequired is true;
            this.IsRequired = required;
            previousRequired = required;
        }
    }

    public void DeriveIsUnique()
    {
        var composites = new Graph<Composite>(this.AssociationType.Composite.Composites, v => v.DirectSubtypes).Reverse();

        bool previousUnique = false;
        foreach (var composite in composites)
        {
            var compositeRoleType = this.CompositeRoleTypeByComposite[composite];
            bool? assignedIsUnique = compositeRoleType.AssignedIsUnique;

            var required = previousUnique || assignedIsUnique is true;
            this.IsUnique = required;
            previousUnique = required;
        }
    }

    public void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this.AssignedWorkspaceNames != null
            ? this.AssignedWorkspaceNames
                .Intersect(this.associationType.Composite.Classes.SelectMany(v => v.WorkspaceNames))
                .Intersect(this.ObjectType switch
                {
                    Unit unit => unit.WorkspaceNames,
                    Interface @interface => @interface.Classes.SelectMany(v => v.WorkspaceNames),
                    Class @class => @class.WorkspaceNames,
                    _ => Array.Empty<string>(),
                })
                .ToArray()
            : Array.Empty<string>();

}

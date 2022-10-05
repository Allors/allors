// <copyright file="RoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Graph;

public abstract class RoleType : IRoleType, IComparable
{
    /// <summary>
    ///     The maximum size value.
    /// </summary>
    public const int MaximumSize = -1;

    protected RoleType(ObjectType objectType, string assignedSingularName, string assignedPluralName)
    {
        this.Attributes = new MetaExtension();
        this.ObjectType = objectType;
        this.AssignedSingularName = !string.IsNullOrEmpty(assignedSingularName) ? assignedSingularName : null;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
    }

    public dynamic Attributes { get; }

    public RelationType RelationType { get; internal set; }

    public ICompositeRoleType CompositeRoleType { get; private set; }

    public IReadOnlyDictionary<IComposite, ICompositeRoleType> CompositeRoleTypeByComposite { get; private set; }

    public AssociationType AssociationType => this.RelationType.AssociationType;

    public ObjectType ObjectType { get; }

    public string AssignedSingularName { get; }

    public string AssignedPluralName { get; }

    public bool ExistAssignedSingularName => this.AssignedSingularName != null;

    public bool ExistAssignedPluralName => this.PluralName != null;

    internal string ValidationName => "RoleType: " + this.RelationType.Name;

    IRelationType IRoleType.RelationType => this.RelationType;

    IAssociationType IRoleType.AssociationType => this.AssociationType;

    IObjectType IPropertyType.ObjectType => this.ObjectType;

    public string SingularName { get; internal set; }

    /// <summary>
    ///     Gets the full singular name.
    /// </summary>
    /// <value>The full singular name.</value>
    public string SingularFullName => this.RelationType.AssociationType.ObjectType + this.SingularName;

    public string PluralName { get; internal set; }

    /// <summary>
    ///     Gets the full plural name.
    /// </summary>
    /// <value>The full plural name.</value>
    public string PluralFullName => this.RelationType.AssociationType.ObjectType + this.PluralName;

    public string Name => this.IsMany ? this.PluralName : this.SingularName;

    public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

    public bool IsMany =>
        this.RelationType.Multiplicity switch
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

    public string MediaType { get; set; }

    public int CompareTo(object other) => this.RelationType.Id.CompareTo((other as RoleType)?.RelationType.Id);

    /// <summary>
    ///     Get the value of the role on this object.
    /// </summary>
    /// <param name="strategy">
    ///     The strategy.
    /// </param>
    /// <returns>
    ///     The role value.
    /// </returns>
    public object Get(IStrategy strategy, IComposite ofType)
    {
        var role = strategy.GetRole(this);

        if (ofType == null || role == null || !this.ObjectType.IsComposite)
        {
            return role;
        }

        if (this.IsOne)
        {
            return ofType.IsAssignableFrom(((IObject)role).Strategy.Class) ? role : null;
        }

        var extent = (IEnumerable<IObject>)role;
        return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
    }

    /// <summary>
    ///     Set the value of the role on this object.
    /// </summary>
    /// <param name="strategy">
    ///     The strategy.
    /// </param>
    /// <param name="value">
    ///     The role value.
    /// </param>
    public void Set(IStrategy strategy, object value) => strategy.SetRole(this, value);

    public override bool Equals(object other) => this.RelationType.Id.Equals((other as RoleType)?.RelationType.Id);

    public override int GetHashCode() => this.RelationType.Id.GetHashCode();

    public override string ToString() => $"{this.AssociationType.ObjectType.Name}.{this.Name}";

    /// <summary>
    ///     Derive multiplicity, scale and size.
    /// </summary>
    internal void DeriveScaleAndSize()
    {
        if (this.ObjectType is IUnit unitType)
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
    internal void Validate(ValidationLog validationLog)
    {
        if (this.ObjectType == null)
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
    }

    internal void InitializeCompositeRoleTypes(Dictionary<IComposite, HashSet<ICompositeRoleType>> compositeRoleTypesByComposite)
    {
        var composites = this.AssociationType.ObjectType.Composites;

        this.CompositeRoleTypeByComposite = composites.ToDictionary(v => v, v =>
        {
            var compositeRoleType = (ICompositeRoleType)new CompositeRoleType(v, this);
            compositeRoleTypesByComposite[v].Add(compositeRoleType);
            return compositeRoleType;
        });

        this.CompositeRoleType = this.CompositeRoleTypeByComposite[this.AssociationType.ObjectType];
    }

    internal void DeriveIsRequired()
    {
        var composites = new Graph<IComposite>(this.AssociationType.ObjectType.Composites, v => v.DirectSubtypes).Reverse();

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

    internal void DeriveIsUnique()
    {
        var composites = new Graph<IComposite>(this.AssociationType.ObjectType.Composites, v => v.DirectSubtypes).Reverse();

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

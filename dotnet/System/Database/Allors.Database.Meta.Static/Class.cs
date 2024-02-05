// <copyright file="Class.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Text;

public abstract class Class : IClass, IStaticComposite, IObjectType, IMetaIdentifiableObject
{
    private ConcurrentDictionary<IMethodType, Action<object, object>[]> actionsByMethodType;

    protected Class(MetaPopulation metaPopulation, Guid id, Interface[] directSupertypes, string singularName, string assignedPluralName)
    {
        // TODO: Create single element IReadOnlyList
        this.Attributes = new MetaExtension();
        this.MetaPopulation = metaPopulation;
        this.Id = id;
        this.Tag = id.Tag();
        this.SingularName = singularName;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
        this.PluralName = this.AssignedPluralName != null ? this.AssignedPluralName : Pluralizer.Pluralize(this.SingularName);
        this.DirectSupertypes = directSupertypes;
        this.Composites = new[] { this };
        this.Classes = new[] { this };
        this.DirectSubtypes = MetaPopulation.EmptyComposites;
        this.Subtypes = MetaPopulation.EmptyComposites;
        metaPopulation.OnCreated(this);
    }

    private IReadOnlyList<IAssociationType> associationTypes;
    private IReadOnlyList<IRoleType> roleTypes;
    private IReadOnlyList<IMethodType> methodTypes;
    private IReadOnlyList<IInterface> supertypes;

    private IReadOnlyDictionary<IRoleType, ICompositeRoleType> compositeRoleTypeByRoleType;
    private IReadOnlyDictionary<IMethodType, ICompositeMethodType> compositeMethodTypeByMethodType;

    private IRoleType derivedKeyRoleType;

    public dynamic Attributes { get; }

    IMetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public MetaPopulation MetaPopulation { get; }

    public Guid Id { get; }

    public string Tag { get; set; }

    public Type BoundType { get; set; }

    public string Name => this.SingularName;

    public string SingularName { get; }

    public string AssignedPluralName { get; }

    public string PluralName { get; }

    public bool IsUnit => this is IUnit;

    public bool IsComposite => this is IComposite;

    public bool IsInterface => this is IInterface;

    public bool IsClass => this is IClass;

    public override bool Equals(object other) => this.Id.Equals((other as IMetaIdentifiableObject)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public int CompareTo(IObjectType other)
    {
        return this.Id.CompareTo(other?.Id);
    }

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return this.SingularName;
        }

        return this.Tag;
    }

    public IReadOnlyList<IInterface> DirectSupertypes { get; }

    IReadOnlyList<IInterface> IComposite.Supertypes => this.supertypes;

    IReadOnlyList<IInterface> IStaticComposite.Supertypes
    {
        get => this.supertypes;
        set => this.supertypes = value;
    }

    IReadOnlyList<IAssociationType> IComposite.AssociationTypes => this.associationTypes;

    IReadOnlyList<IAssociationType> IStaticComposite.AssociationTypes
    {
        get => this.associationTypes;
        set => this.associationTypes = value;
    }

    IReadOnlyList<IRoleType> IComposite.RoleTypes => this.roleTypes;

    IReadOnlyList<IRoleType> IStaticComposite.RoleTypes
    {
        get => this.roleTypes;
        set => this.roleTypes = value;
    }

    public IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType => this.compositeRoleTypeByRoleType;

    IReadOnlyDictionary<IRoleType, ICompositeRoleType> IStaticComposite.CompositeRoleTypeByRoleType
    {
        get => this.compositeRoleTypeByRoleType;
        set => this.compositeRoleTypeByRoleType = value;
    }

    public IRoleType KeyRoleType => this.derivedKeyRoleType;

    public IReadOnlyList<IMethodType> MethodTypes => this.methodTypes;

    IReadOnlyList<IMethodType> IStaticComposite.MethodTypes
    {
        get => this.methodTypes;
        set => this.methodTypes = value;
    }

    public IReadOnlyDictionary<IMethodType, ICompositeMethodType> CompositeMethodTypeByMethodType => this.compositeMethodTypeByMethodType;

    IRoleType IStaticComposite.DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    IReadOnlyDictionary<IMethodType, ICompositeMethodType> IStaticComposite.CompositeMethodTypeByMethodType
    {
        get => this.compositeMethodTypeByMethodType;
        set => this.compositeMethodTypeByMethodType = value;
    }

    public void Validate(ValidationLog validationLog)
    {
        this.ValidateObjectType(validationLog);
        this.ValidateComposite(validationLog);
    }
    
    public string[] AssignedWorkspaceNames { get; set; } = Array.Empty<string>();

    public IReadOnlyList<IComposite> Composites { get; }

    public IReadOnlyList<IClass> Classes { get; }

    public IClass ExclusiveClass => this;

    public IReadOnlyList<IComposite> DirectSubtypes { get; }

    public IReadOnlyList<IComposite> Subtypes { get; }

    public IEnumerable<string> WorkspaceNames => this.AssignedWorkspaceNames;

    public bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);
}

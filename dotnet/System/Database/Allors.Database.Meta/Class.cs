// <copyright file="Class.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Allors.Embedded;
using Allors.Embedded.Meta;

public sealed class Class : EmbeddedObject, IComposite
{
    private readonly IEmbeddedUnitRole<string> singularName;
    private readonly IEmbeddedUnitRole<string> assignedPluralName;
    private readonly IEmbeddedUnitRole<string> pluralName;

    private ConcurrentDictionary<MethodType, Action<object, object>[]> actionsByMethodType;

    public Class(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.MetaPopulation = metaPopulation;

        this.Attributes = new MetaExtension();

        this.singularName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeSingularName);
        this.assignedPluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeAssignedPluralName);
        this.pluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypePluralName);

        this.Composites = new[] { this };
        this.Classes = new[] { this };
        this.DirectSubtypes = Array.Empty<IComposite>();
        this.Subtypes = Array.Empty<IComposite>();

        this.MetaPopulation.OnCreated(this);
    }

    private IReadOnlyList<AssociationType> associationTypes;
    private IReadOnlyList<RoleType> roleTypes;
    private IReadOnlyList<MethodType> methodTypes;
    private IReadOnlyList<Interface> supertypes;

    private IReadOnlyDictionary<RoleType, CompositeRoleType> compositeRoleTypeByRoleType;
    private IReadOnlyDictionary<MethodType, CompositeMethodType> compositeMethodTypeByMethodType;

    private RoleType derivedKeyRoleType;

    public dynamic Attributes { get; }

    MetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public MetaPopulation MetaPopulation { get; }
    
    public Guid Id { get; set; }

    public string Tag { get; set; }

    public Type BoundType { get; set; }

    public string SingularName { get => this.singularName.Value; set => this.singularName.Value = value; }

    public string AssignedPluralName { get => this.assignedPluralName.Value; set => this.assignedPluralName.Value = value; }

    public string PluralName { get => this.pluralName.Value; set => this.pluralName.Value = value; }

    public bool IsUnit => false;

    public bool IsComposite => true;

    public bool IsInterface => false;

    public bool IsClass => true;

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

    public IReadOnlyList<Interface> DirectSupertypes { get; set; }

    public IReadOnlyList<Interface> Supertypes
    {
        get => this.supertypes;
        set => this.supertypes = value;
    }

   public IReadOnlyList<AssociationType> AssociationTypes
    {
        get => this.associationTypes;
        set => this.associationTypes = value;
    }

    public IReadOnlyList<RoleType> RoleTypes
    {
        get => this.roleTypes;
        set => this.roleTypes = value;
    }

    public IReadOnlyDictionary<RoleType, CompositeRoleType> CompositeRoleTypeByRoleType
    {
        get => this.compositeRoleTypeByRoleType;
        set => this.compositeRoleTypeByRoleType = value;
    }

    public RoleType KeyRoleType => this.derivedKeyRoleType;

    public IReadOnlyList<MethodType> MethodTypes
    {
        get => this.methodTypes;
        set => this.methodTypes = value;
    }

    public RoleType DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    public IReadOnlyDictionary<MethodType, CompositeMethodType> CompositeMethodTypeByMethodType
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

    public IReadOnlyList<Class> Classes { get; }

    public Class ExclusiveClass => this;

    public IReadOnlyList<IComposite> DirectSubtypes { get; }

    public IReadOnlyList<IComposite> Subtypes { get; }

    public IEnumerable<string> WorkspaceNames => this.AssignedWorkspaceNames;

    public bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);
}

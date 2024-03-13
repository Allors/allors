// <copyright file="IComposite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Embedded;
using Embedded.Meta;

public abstract class Composite : ObjectType
{
    private RoleType derivedKeyRoleType;

    protected Composite(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType) 
        : base(metaPopulation, embeddedObjectType)
    {
        this.DirectSupertypes = Array.Empty<Interface>();
    }

    public override bool IsUnit => false;

    public override bool IsComposite => true;

    public IReadOnlyList<Interface> DirectSupertypes { get; set; }

    public IReadOnlyList<Interface> Supertypes { get; internal set; }

    public abstract IReadOnlyList<Composite> DirectSubtypes { get; }

    public abstract IReadOnlyList<Composite> Subtypes { get; }

    public abstract IReadOnlyList<Composite> Composites { get; }

    public abstract IReadOnlyList<Class> Classes { get; }

    public abstract Class ExclusiveClass { get; }

    public IReadOnlyList<AssociationType> AssociationTypes { get; internal set; }

    public IReadOnlyList<RoleType> RoleTypes { get; internal set; }

    public IReadOnlyDictionary<RoleType, CompositeRoleType> CompositeRoleTypeByRoleType { get; internal set; }

    public IReadOnlyList<MethodType> MethodTypes { get; internal set; }

    public IReadOnlyDictionary<MethodType, CompositeMethodType> CompositeMethodTypeByMethodType { get; internal set; }

    public RoleType KeyRoleType => this.derivedKeyRoleType;

    internal RoleType DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    public abstract bool IsAssignableFrom(Composite objectType);

    internal void ValidateComposite(ValidationLog validationLog)
    {
        if (this.RoleTypes.Count(v => v.IsKey) > 1)
        {
            var message = this.ValidationName() + " has more than 1 key";
            validationLog.AddError(message, this, ValidationKind.Multiplicity, "IComposite.KeyRoleType");
        }
    }

    internal void InitializeSupertypes()
    {
        var supertypes = new HashSet<Interface>();
        this.InitializeSupertypesRecursively(this, supertypes);
        this.Supertypes = supertypes.ToArray();
    }

    internal void InitializeRoleTypes(Dictionary<Composite, HashSet<RoleType>> roleTypesByAssociationObjectType)
    {
        var roleTypes = new HashSet<RoleType>();

        if (roleTypesByAssociationObjectType.TryGetValue(this, out var directRoleTypes))
        {
            roleTypes.UnionWith(directRoleTypes);
        }

        foreach (var superType in this.Supertypes.Cast<Composite>())
        {
            if (roleTypesByAssociationObjectType.TryGetValue(superType, out var inheritedRoleTypes))
            {
                roleTypes.UnionWith(inheritedRoleTypes);
            }
        }

        this.RoleTypes = roleTypes.ToArray();
    }

    internal void InitializeAssociationTypes(Dictionary<ObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
    {
        var associationTypes = new HashSet<AssociationType>();

        if (relationTypesByRoleObjectType.TryGetValue(this, out var classAssociationTypes))
        {
            associationTypes.UnionWith(classAssociationTypes);
        }

        foreach (var superType in this.Supertypes.Cast<Interface>())
        {
            if (relationTypesByRoleObjectType.TryGetValue(superType, out var interfaceAssociationTypes))
            {
                associationTypes.UnionWith(interfaceAssociationTypes);
            }
        }

        this.AssociationTypes = associationTypes.ToArray();
    }

    internal void InitializeMethodTypes(Dictionary<Composite, HashSet<MethodType>> methodTypeByClass)
    {
        var methodTypes = new HashSet<MethodType>();

        if (methodTypeByClass.TryGetValue(this, out var directMethodTypes))
        {
            methodTypes.UnionWith(directMethodTypes);
        }

        foreach (var superType in this.Supertypes.Cast<Interface>())
        {
            if (methodTypeByClass.TryGetValue(superType, out var inheritedMethodTypes))
            {
                methodTypes.UnionWith(inheritedMethodTypes);
            }
        }

        this.MethodTypes = methodTypes.ToArray();
    }

    internal void InitializeCompositeRoleTypes(Dictionary<Composite, HashSet<CompositeRoleType>> compositeRoleTypesByComposite)
    {
        var compositeRoleTypes = compositeRoleTypesByComposite[this];
        this.CompositeRoleTypeByRoleType = compositeRoleTypes.ToDictionary(v => v.RoleType, v => v);
    }

    internal void InitializeCompositeMethodTypes(Dictionary<Composite, HashSet<CompositeMethodType>> compositeMethodTypesByComposite)
    {
        var compositeMethodTypes = compositeMethodTypesByComposite[this];
        this.CompositeMethodTypeByMethodType = compositeMethodTypes.ToDictionary(v => v.MethodType, v => v);
    }

    internal void InitializeSupertypesRecursively(ObjectType type, ISet<Interface> superTypes)
    {
        foreach (var directSupertype in this.DirectSupertypes.Cast<Interface>())
        {
            if (!Equals(directSupertype, type))
            {
                superTypes.Add(directSupertype);
                directSupertype.InitializeSupertypesRecursively(type, superTypes);
            }
        }
    }

    internal void DeriveKeyRoleType()
    {
        this.DerivedKeyRoleType = this.RoleTypes.FirstOrDefault(v => v.IsKey);
    }
}

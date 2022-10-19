﻿// <copyright file="Composite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Composite : ObjectType, IComposite
{
    private IReadOnlyList<IAssociationType> associationTypes;
    private IReadOnlyList<IRoleType> roleTypes;
    private IReadOnlyList<IMethodType> methodTypes;
    private IReadOnlyList<IInterface> supertypes;

    protected Composite(MetaPopulation metaPopulation, Guid id, IReadOnlyList<IInterface> directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, singularName, assignedPluralName)
    {
        this.DirectSupertypes = directSupertypes;
    }

    public IReadOnlyList<IInterface> DirectSupertypes { get; }

    public abstract IReadOnlyList<IComposite> DirectSubtypes { get; }

    public abstract IReadOnlyList<IComposite> Subtypes { get; }

    public abstract IReadOnlyList<IComposite> Composites { get; }

    public abstract IReadOnlyList<IClass> Classes { get; }

    public abstract IClass ExclusiveClass { get; }

    public IReadOnlyList<IInterface> Supertypes => this.supertypes;

    public IReadOnlyList<IAssociationType> AssociationTypes => this.associationTypes;

    public IReadOnlyList<IRoleType> RoleTypes => this.roleTypes;

    public IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType { get; private set; }

    public IReadOnlyList<IMethodType> MethodTypes => this.methodTypes;

    public IReadOnlyDictionary<IMethodType, ICompositeMethodType> CompositeMethodTypeByMethodType { get; private set; }

    public abstract bool IsAssignableFrom(IComposite objectType);

    internal void InitializeSupertypes()
    {
        var supertypes = new HashSet<IInterface>();
        this.InitializeSupertypesRecursively(this, supertypes);
        this.supertypes = supertypes.ToArray();
    }

    internal void InitializeRoleTypes(Dictionary<Composite, HashSet<RoleType>> roleTypesByAssociationObjectType)
    {
        var roleTypes = new HashSet<RoleType>();

        if (roleTypesByAssociationObjectType.TryGetValue(this, out var directRoleTypes))
        {
            roleTypes.UnionWith(directRoleTypes);
        }

        foreach (var superType in this.Supertypes.Cast<Interface>())
        {
            if (roleTypesByAssociationObjectType.TryGetValue(superType, out var inheritedRoleTypes))
            {
                roleTypes.UnionWith(inheritedRoleTypes);
            }
        }

        this.roleTypes = roleTypes.ToArray();
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

        this.associationTypes = associationTypes.ToArray();
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

        this.methodTypes = methodTypes.ToArray();
    }

    internal void InitializeCompositeRoleTypes(Dictionary<IComposite, HashSet<ICompositeRoleType>> compositeRoleTypesByComposite)
    {
        var compositeRoleTypes = compositeRoleTypesByComposite[this];
        this.CompositeRoleTypeByRoleType = compositeRoleTypes.ToDictionary(v => v.RoleType, v => v);
    }

    internal void InitializeCompositeMethodTypes(Dictionary<IComposite, HashSet<ICompositeMethodType>> compositeMethodTypesByComposite)
    {
        var compositeMethodTypes = compositeMethodTypesByComposite[this];
        this.CompositeMethodTypeByMethodType = compositeMethodTypes.ToDictionary(v => v.MethodType, v => v);
    }

    private void InitializeSupertypesRecursively(ObjectType type, ISet<IInterface> superTypes)
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
}

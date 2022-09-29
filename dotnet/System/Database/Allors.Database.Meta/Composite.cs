// <copyright file="Composite.cs" company="Allors bvba">
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
    private IReadOnlySet<IAssociationType> associationTypes;
    private IReadOnlySet<IRoleType> roleTypes;
    private IReadOnlySet<IMethodType> methodTypes;
    private HashSet<IInterface> supertypes;

    protected Composite(MetaPopulation metaPopulation, Guid id, IEnumerable<IInterface> directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, singularName, assignedPluralName)
    {
        this.DirectSupertypes = new HashSet<IInterface>(directSupertypes);
    }

    public IReadOnlySet<IInterface> DirectSupertypes { get; }

    public abstract IReadOnlySet<IComposite> Subtypes { get; }

    public abstract IReadOnlySet<IClass> Classes { get; }

    public abstract IClass ExclusiveClass { get; }

    public IReadOnlySet<IInterface> Supertypes => this.supertypes;

    public IReadOnlySet<IAssociationType> AssociationTypes => this.associationTypes;

    public IReadOnlySet<IRoleType> RoleTypes => this.roleTypes;

    public IReadOnlySet<IMethodType> MethodTypes => this.methodTypes;

    public bool ExistExclusiveClass => this.ExclusiveClass != null;

    public bool ExistSupertype(IInterface @interface) => this.supertypes.Contains(@interface);

    public bool ExistAssociationType(IAssociationType associationType) => this.associationTypes.Contains(associationType);

    public bool ExistRoleType(IRoleType roleType) => this.roleTypes.Contains(roleType);

    public abstract bool IsAssignableFrom(IComposite objectType);

    internal void InitializeSupertypes()
    {
        var supertypes = new HashSet<IInterface>();
        this.InitializeSupertypesRecursively(this, supertypes);
        this.supertypes = new HashSet<IInterface>(supertypes);
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

        this.roleTypes = new HashSet<IRoleType>(roleTypes);
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

        this.associationTypes = new HashSet<IAssociationType>(associationTypes);
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

        this.methodTypes = new HashSet<IMethodType>(methodTypes);
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

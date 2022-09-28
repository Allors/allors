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
    private HashSet<AssociationType> associationTypes;

    private HashSet<MethodType> methodTypes;
    private HashSet<RoleType> roleTypes;
    private HashSet<Interface> supertypes;

    protected Composite(MetaPopulation metaPopulation, Guid id, Interface[] directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, singularName, assignedPluralName)
    {
        this.DirectSupertypes = directSupertypes;
    }

    IEnumerable<IInterface> IComposite.DirectSupertypes => this.DirectSupertypes;

    public Interface[] DirectSupertypes { get; }

    IEnumerable<IComposite> IComposite.Subtypes => this.Subtypes;

    public abstract IEnumerable<Composite> Subtypes { get; }

    IEnumerable<IClass> IComposite.Classes => this.Classes;

    public abstract IEnumerable<Class> Classes { get; }

    IClass IComposite.ExclusiveClass => this.ExclusiveClass;

    public abstract Class ExclusiveClass { get; }

    IEnumerable<IInterface> IComposite.Supertypes => this.Supertypes;

    public IEnumerable<Interface> Supertypes => this.supertypes;

    IEnumerable<IAssociationType> IComposite.AssociationTypes => this.AssociationTypes;

    public IEnumerable<AssociationType> AssociationTypes => this.associationTypes;

    IEnumerable<IRoleType> IComposite.RoleTypes => this.RoleTypes;

    public IEnumerable<RoleType> RoleTypes => this.roleTypes;

    IEnumerable<IMethodType> IComposite.MethodTypes => this.MethodTypes;

    public IEnumerable<MethodType> MethodTypes => this.methodTypes;

    public abstract bool ExistClass { get; }

    public bool ExistExclusiveClass => this.ExclusiveClass != null;

    public bool ExistSupertype(IInterface @interface) => this.supertypes.Contains(@interface);

    public bool ExistAssociationType(IAssociationType associationType) => this.associationTypes.Contains(associationType);

    public bool ExistRoleType(IRoleType roleType) => this.roleTypes.Contains(roleType);

    public abstract bool IsAssignableFrom(IComposite objectType);

    internal void InitializeSupertypes()
    {
        var supertypes = new HashSet<Interface>();
        this.InitializeSupertypesRecursively(this, supertypes);
        this.supertypes = new HashSet<Interface>(supertypes);
    }

    internal void InitializeRoleTypes(Dictionary<Composite, HashSet<RoleType>> roleTypesByAssociationObjectType)
    {
        var roleTypes = new HashSet<RoleType>();

        if (roleTypesByAssociationObjectType.TryGetValue(this, out var directRoleTypes))
        {
            roleTypes.UnionWith(directRoleTypes);
        }

        foreach (var superType in this.Supertypes)
        {
            if (roleTypesByAssociationObjectType.TryGetValue(superType, out var inheritedRoleTypes))
            {
                roleTypes.UnionWith(inheritedRoleTypes);
            }
        }

        this.roleTypes = new HashSet<RoleType>(roleTypes);
    }

    internal void InitializeAssociationTypes(Dictionary<ObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
    {
        var associationTypes = new HashSet<AssociationType>();

        if (relationTypesByRoleObjectType.TryGetValue(this, out var classAssociationTypes))
        {
            associationTypes.UnionWith(classAssociationTypes);
        }

        foreach (var superType in this.Supertypes)
        {
            if (relationTypesByRoleObjectType.TryGetValue(superType, out var interfaceAssociationTypes))
            {
                associationTypes.UnionWith(interfaceAssociationTypes);
            }
        }

        this.associationTypes = new HashSet<AssociationType>(associationTypes);
    }

    internal void InitializeMethodTypes(Dictionary<Composite, HashSet<MethodType>> methodTypeByClass)
    {
        var methodTypes = new HashSet<MethodType>();

        if (methodTypeByClass.TryGetValue(this, out var directMethodTypes))
        {
            methodTypes.UnionWith(directMethodTypes);
        }

        foreach (var superType in this.Supertypes)
        {
            if (methodTypeByClass.TryGetValue(superType, out var inheritedMethodTypes))
            {
                methodTypes.UnionWith(inheritedMethodTypes);
            }
        }

        this.methodTypes = new HashSet<MethodType>(methodTypes);
    }

    private void InitializeSupertypesRecursively(ObjectType type, HashSet<Interface> superTypes)
    {
        foreach (var directSupertype in this.DirectSupertypes)
        {
            if (!Equals(directSupertype, type))
            {
                superTypes.Add(directSupertype);
                directSupertype.InitializeSupertypesRecursively(type, superTypes);
            }
        }
    }
}

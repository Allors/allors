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

    public abstract IEnumerable<Class> Classes { get; }

    public abstract Class ExclusiveClass { get; }

    IEnumerable<IInterface> IComposite.DirectSupertypes => this.DirectSupertypes;

    public Interface[] DirectSupertypes { get; }

    public IEnumerable<Interface> Supertypes => this.supertypes;

    public IEnumerable<AssociationType> AssociationTypes => this.associationTypes;

    public IEnumerable<AssociationType> ExclusiveAssociationTypes => this.AssociationTypes.Where(associationType => this.Equals(associationType.RoleType.ObjectType)).ToArray();

    public IEnumerable<AssociationType> ExclusiveDatabaseAssociationTypes => this.ExclusiveAssociationTypes.ToArray();

    public IEnumerable<AssociationType> InheritedAssociationTypes => this.AssociationTypes.Except(this.ExclusiveAssociationTypes);

    public IEnumerable<RoleType> RoleTypes => this.roleTypes;

    public IEnumerable<RoleType> ExclusiveRoleTypes =>
        this.RoleTypes.Where(roleType => this.Equals(roleType.AssociationType.ObjectType)).ToArray();

    public IEnumerable<MethodType> MethodTypes => this.methodTypes;

    public IEnumerable<MethodType> ExclusiveMethodTypes =>
        this.MethodTypes.Where(methodType => this.Equals(methodType.ObjectType)).ToArray();

    public IEnumerable<MethodType> InheritedMethodTypes => this.MethodTypes.Except(this.ExclusiveMethodTypes);

    public IEnumerable<RoleType> InheritedRoleTypes => this.RoleTypes.Except(this.ExclusiveRoleTypes);

    public abstract IEnumerable<Composite> Subtypes { get; }

    public abstract bool ExistClass { get; }

    public bool ExistExclusiveClass => this.ExclusiveClass != null;

    IClass IComposite.ExclusiveClass => this.ExclusiveClass;

    IEnumerable<IInterface> IComposite.Supertypes => this.Supertypes;

    IEnumerable<IAssociationType> IComposite.AssociationTypes => this.AssociationTypes;

    IEnumerable<IAssociationType> IComposite.ExclusiveAssociationTypes => this.ExclusiveDatabaseAssociationTypes;

    IEnumerable<IAssociationType> IComposite.InheritedAssociationTypes => this.InheritedAssociationTypes;

    IEnumerable<IRoleType> IComposite.RoleTypes => this.RoleTypes;

    IEnumerable<IRoleType> IComposite.ExclusiveRoleTypes => this.ExclusiveRoleTypes;

    IEnumerable<IMethodType> IComposite.MethodTypes => this.MethodTypes;

    IEnumerable<IMethodType> IComposite.ExclusiveMethodTypes => this.ExclusiveMethodTypes;

    IEnumerable<IMethodType> IComposite.InheritedMethodTypes => this.InheritedMethodTypes;

    IEnumerable<IRoleType> IComposite.InheritedRoleTypes => this.InheritedRoleTypes;

    IEnumerable<IComposite> IComposite.Subtypes => this.Subtypes;

    IEnumerable<IClass> IComposite.Classes => this.Classes;

    public bool ExistSupertype(IInterface @interface) => this.supertypes.Contains(@interface);

    public bool ExistAssociationType(IAssociationType associationType) => this.associationTypes.Contains(associationType);

    public bool ExistRoleType(IRoleType roleType) => this.roleTypes.Contains(roleType);

    public abstract bool IsAssignableFrom(IComposite objectType);

    internal void InitializeSupertypes(HashSet<Interface> superTypes)
    {
        superTypes.Clear();
        this.InitializeSupertypesRecursively(this, superTypes);
        this.supertypes = new HashSet<Interface>(superTypes);
    }

    internal void InitializeRoleTypes(HashSet<RoleType> roleTypes, Dictionary<Composite, HashSet<RoleType>> roleTypesByAssociationObjectType)
    {
        roleTypes.Clear();

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

    internal void InitializeAssociationTypes(HashSet<AssociationType> associationTypes, Dictionary<ObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
    {
        associationTypes.Clear();

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

    internal void InitializeMethodTypes(HashSet<MethodType> methodTypes, Dictionary<Composite, HashSet<MethodType>> methodTypeByClass)
    {
        methodTypes.Clear();

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

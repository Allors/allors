// <copyright file="IComposite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
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

    public static implicit operator Composite(ICompositeIndex index) => index.Composite;

    public static implicit operator Composite(IInterfaceIndex index) => index.Interface;

    public static implicit operator Composite(IClassIndex index) => index.Class;
}

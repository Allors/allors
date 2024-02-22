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

public abstract class Composite : EmbeddedObject, IObjectType
{
    private readonly IEmbeddedUnitRole<string> singularName;
    private readonly IEmbeddedUnitRole<string> assignedPluralName;
    private readonly IEmbeddedUnitRole<string> pluralName;
    
    protected Composite(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType) 
        : base(metaPopulation, embeddedObjectType)
    {
        this.MetaPopulation = metaPopulation;

        this.Attributes = new MetaExtension();
        this.DirectSupertypes = Array.Empty<Interface>();

        this.singularName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeSingularName);
        this.assignedPluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeAssignedPluralName);
        this.pluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypePluralName);
    }

    public MetaPopulation MetaPopulation { get; }

    public dynamic Attributes { get; }

    public Guid Id { get; set; }

    public string Tag { get; set; }

    public Type BoundType { get; set; }

    public string SingularName { get => this.singularName.Value; set => this.singularName.Value = value; }

    public string AssignedPluralName { get => this.assignedPluralName.Value; set => this.assignedPluralName.Value = value; }

    public string PluralName { get => this.pluralName.Value; set => this.pluralName.Value = value; }

    public bool IsUnit => false;

    public bool IsComposite => true;

    public abstract bool IsInterface { get; }

    public abstract bool IsClass { get; }

    public abstract IEnumerable<string> WorkspaceNames { get; }

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

    public abstract RoleType KeyRoleType { get; }

    public IReadOnlyList<MethodType> MethodTypes { get; internal set; }

    public IReadOnlyDictionary<MethodType, CompositeMethodType> CompositeMethodTypeByMethodType { get; internal set; }

    public abstract bool IsAssignableFrom(Composite objectType);

    internal RoleType DerivedKeyRoleType { get; set; }

    public static implicit operator Composite(ICompositeIndex index) => index.Composite;

    public static implicit operator Composite(IInterfaceIndex index) => index.Interface;

    public static implicit operator Composite(IClassIndex index) => index.Class;

    public abstract void Validate(ValidationLog validationLog);

    public int CompareTo(IObjectType other)
    {
        return this.Id.CompareTo(other?.Id);
    }
}

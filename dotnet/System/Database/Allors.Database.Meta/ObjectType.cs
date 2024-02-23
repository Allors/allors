// <copyright file="IObjectType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Allors.Embedded;
using Embedded.Meta;

public abstract class ObjectType : OperandType, IMetaIdentifiableObject, IComparable<ObjectType>
{
    private readonly IEmbeddedUnitRole<string> singularName;
    private readonly IEmbeddedUnitRole<string> assignedPluralName;
    private readonly IEmbeddedUnitRole<string> pluralName;
    
    protected ObjectType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType) 
        : base(metaPopulation, embeddedObjectType)
    {
        this.Attributes = new MetaExtension();

        this.MetaPopulation = metaPopulation;

        this.singularName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeSingularName);
        this.assignedPluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeAssignedPluralName);
        this.pluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypePluralName);

    }

    public MetaPopulation MetaPopulation { get; }

    public dynamic Attributes { get; }

    public Guid Id { get; set; }

    public string Tag { get; set; }

    public Type BoundType { get; internal set; }

    public string SingularName { get => this.singularName.Value; set => this.singularName.Value = value; }

    public string AssignedPluralName { get => this.assignedPluralName.Value; set => this.assignedPluralName.Value = value; }

    public string PluralName { get => this.pluralName.Value; set => this.pluralName.Value = value; }

    public abstract IEnumerable<string> WorkspaceNames { get; }

    public abstract bool IsUnit { get; }

    public abstract bool IsComposite { get; }

    public abstract bool IsInterface { get; }

    public abstract bool IsClass { get; }

    public static implicit operator ObjectType(UnitIndex index) => index.Unit;

    public static implicit operator ObjectType(CompositeIndex index) => index.Composite;

    public static implicit operator ObjectType(InterfaceIndex index) => index.Interface;

    public static implicit operator ObjectType(ClassIndex index) => index.Class;

    public int CompareTo(ObjectType other)
    {
        return this.Id.CompareTo(other?.Id);
    }

    public abstract void Validate(ValidationLog validationLog);
}

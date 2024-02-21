// <copyright file="Interface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Embedded;
using Allors.Embedded.Meta;
using Text;

public sealed class Interface : EmbeddedObject, IComposite
{
    private readonly IEmbeddedUnitRole<string> singularName;
    private readonly IEmbeddedUnitRole<string> assignedPluralName;
    private readonly IEmbeddedUnitRole<string> pluralName;

    private string[] derivedWorkspaceNames;

    private IReadOnlyList<IComposite> composites;
    private IReadOnlyList<IComposite> directSubtypes;
    private IReadOnlyList<IComposite> subtypes;
    private IReadOnlyList<IClass> subclasses;
    private IClass exclusiveClass;

    public Interface(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.Attributes = new MetaExtension();
        this.MetaPopulation = metaPopulation;

        this.singularName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeSingularName);
        this.assignedPluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypeAssignedPluralName);
        this.pluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.EmbeddedRoleTypes.ObjectTypePluralName);
        
        this.DirectSupertypes = Array.Empty<Interface>();

        metaPopulation.OnCreated(this);
    }
    
    private IReadOnlyList<IAssociationType> associationTypes;
    private IReadOnlyList<IRoleType> roleTypes;
    private IReadOnlyList<MethodType> methodTypes;
    private IReadOnlyList<Interface> supertypes;

    private IReadOnlyDictionary<IRoleType, ICompositeRoleType> compositeRoleTypeByRoleType;
    private IReadOnlyDictionary<MethodType, ICompositeMethodType> compositeMethodTypeByMethodType;

    private IRoleType derivedKeyRoleType;

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

    public bool IsInterface => true;

    public bool IsClass => false;

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

    public IReadOnlyList<IAssociationType> AssociationTypes
    {
        get => this.associationTypes;
        set => this.associationTypes = value;
    }

    public IReadOnlyList<IRoleType> RoleTypes
    {
        get => this.roleTypes;
        set => this.roleTypes = value;
    }

    public IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType
    {
        get => this.compositeRoleTypeByRoleType;
        set => this.compositeRoleTypeByRoleType = value;
    }

    public IRoleType KeyRoleType => this.derivedKeyRoleType;

    public IReadOnlyList<MethodType> MethodTypes
    {
        get => this.methodTypes;
        set => this.methodTypes = value;
    }

    public IRoleType DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    public IReadOnlyDictionary<MethodType, ICompositeMethodType> CompositeMethodTypeByMethodType
    {
        get => this.compositeMethodTypeByMethodType;
        set => this.compositeMethodTypeByMethodType = value;
    }

    public void Validate(ValidationLog validationLog)
    {
        this.ValidateObjectType(validationLog);
        this.ValidateComposite(validationLog);
    }


    public IReadOnlyList<IComposite> DirectSubtypes => this.directSubtypes;

    public IReadOnlyList<IComposite> Composites => this.composites;

    public IReadOnlyList<IClass> Classes => this.subclasses;

    public IReadOnlyList<IComposite> Subtypes => this.subtypes;

    public IClass ExclusiveClass => this.exclusiveClass;

    public IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    public bool IsAssignableFrom(IComposite objectType) =>
        this.Equals(objectType) || this.subtypes.Contains(objectType);

    public void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = ((Interface)this)
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(((Interface)this).AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    public void InitializeDirectSubtypes()
    {
        this.directSubtypes = this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)).ToArray();
    }

    public void InitializeSubtypes()
    {
        var subtypes = new HashSet<IComposite>();
        this.InitializeSubtypesRecursively(this, subtypes);
        this.subtypes = subtypes.ToArray();
    }

    public void InitializeSubclasses()
    {
        var subclasses = new HashSet<Class>();
        foreach (var subType in this.subtypes.OfType<IClass>())
        {
            subclasses.Add((Class)subType);
        }

        this.subclasses = subclasses.ToArray();
    }

    public void InitializeComposites()
    {
        this.composites = this.subtypes.Append(this).ToArray();
    }

    public void InitializeExclusiveSubclass() => this.exclusiveClass = this.subclasses.Count == 1 ? this.subclasses.First() : null;

    private void InitializeSubtypesRecursively(IObjectType type, ISet<IComposite> subtypes)
    {
        foreach (var directSubtype in this.DirectSubtypes.Cast<IComposite>())
        {
            if (!Equals(directSubtype, type))
            {
                subtypes.Add(directSubtype);
                if (directSubtype is Interface directSubinterface)
                {
                    directSubinterface.InitializeSubtypesRecursively(this, subtypes);
                }
            }
        }
    }
}

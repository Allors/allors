// <copyright file="Interface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Text;

public abstract class Interface : IStaticInterface, IStaticComposite, IObjectType, IMetaIdentifiableObject
{
    private string[] derivedWorkspaceNames;

    private IReadOnlyList<IComposite> composites;
    private IReadOnlyList<IComposite> directSubtypes;
    private IReadOnlyList<IComposite> subtypes;
    private IReadOnlyList<IClass> subclasses;
    private IClass exclusiveClass;

    protected Interface(IStaticMetaPopulation metaPopulation, Guid id, IReadOnlyList<Interface> directSupertypes, string singularName, string assignedPluralName)
    {
        this.Attributes = new MetaExtension();
        this.MetaPopulation = metaPopulation;
        this.Id = id;
        this.Tag = id.Tag();
        this.SingularName = singularName;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
        this.PluralName = this.AssignedPluralName != null ? this.AssignedPluralName : Pluralizer.Pluralize(this.SingularName);
        this.DirectSupertypes = directSupertypes;
        metaPopulation.OnCreated(this);
    }
    
    private IReadOnlyList<IAssociationType> associationTypes;
    private IReadOnlyList<IRoleType> roleTypes;
    private IReadOnlyList<IMethodType> methodTypes;
    private IReadOnlyList<IInterface> supertypes;

    private IReadOnlyDictionary<IRoleType, ICompositeRoleType> compositeRoleTypeByRoleType;
    private IReadOnlyDictionary<IMethodType, ICompositeMethodType> compositeMethodTypeByMethodType;

    private IRoleType derivedKeyRoleType;

    public dynamic Attributes { get; }

    IMetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public IStaticMetaPopulation MetaPopulation { get; }

    public Guid Id { get; }

    public string Tag { get; set; }

    public Type BoundType { get; set; }

    string IObjectType.Name => this.SingularName;

    public string SingularName { get; }

    public string AssignedPluralName { get; }

    public string PluralName { get; }

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

    public IReadOnlyList<IInterface> DirectSupertypes { get; }

    IReadOnlyList<IInterface> IComposite.Supertypes => this.supertypes;

    IReadOnlyList<IInterface> IStaticComposite.Supertypes
    {
        get => this.supertypes;
        set => this.supertypes = value;
    }

    IReadOnlyList<IAssociationType> IComposite.AssociationTypes => this.associationTypes;

    IReadOnlyList<IAssociationType> IStaticComposite.AssociationTypes
    {
        get => this.associationTypes;
        set => this.associationTypes = value;
    }

    IReadOnlyList<IRoleType> IComposite.RoleTypes => this.roleTypes;

    IReadOnlyList<IRoleType> IStaticComposite.RoleTypes
    {
        get => this.roleTypes;
        set => this.roleTypes = value;
    }

    public IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType => this.compositeRoleTypeByRoleType;

    IReadOnlyDictionary<IRoleType, ICompositeRoleType> IStaticComposite.CompositeRoleTypeByRoleType
    {
        get => this.compositeRoleTypeByRoleType;
        set => this.compositeRoleTypeByRoleType = value;
    }

    public IRoleType KeyRoleType => this.derivedKeyRoleType;

    public IReadOnlyList<IMethodType> MethodTypes => this.methodTypes;

    IReadOnlyList<IMethodType> IStaticComposite.MethodTypes
    {
        get => this.methodTypes;
        set => this.methodTypes = value;
    }

    public IReadOnlyDictionary<IMethodType, ICompositeMethodType> CompositeMethodTypeByMethodType => this.compositeMethodTypeByMethodType;

    IRoleType IStaticComposite.DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    IReadOnlyDictionary<IMethodType, ICompositeMethodType> IStaticComposite.CompositeMethodTypeByMethodType
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

    void IStaticInterface.DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = ((IInterface)this)
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(((IInterface)this).AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    void IStaticInterface.InitializeDirectSubtypes()
    {
        this.directSubtypes = this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)).ToArray();
    }

    void IStaticInterface.InitializeSubtypes()
    {
        var subtypes = new HashSet<IComposite>();
        this.InitializeSubtypesRecursively(this, subtypes);
        this.subtypes = subtypes.ToArray();
    }

    void IStaticInterface.InitializeSubclasses()
    {
        var subclasses = new HashSet<Class>();
        foreach (var subType in this.subtypes.OfType<IClass>())
        {
            subclasses.Add((Class)subType);
        }

        this.subclasses = subclasses.ToArray();
    }

    void IStaticInterface.InitializeComposites()
    {
        this.composites = this.subtypes.Append(this).ToArray();
    }

    void IStaticInterface.InitializeExclusiveSubclass() => this.exclusiveClass = this.subclasses.Count == 1 ? this.subclasses.First() : null;

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

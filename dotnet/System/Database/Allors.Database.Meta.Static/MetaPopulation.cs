// <copyright file="MetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

public abstract class MetaPopulation : IMetaPopulation
{
    internal static readonly IReadOnlyList<IComposite> EmptyComposites = Array.Empty<IComposite>();
    internal static readonly IReadOnlyList<IDomain> EmptyDomains = Array.Empty<IDomain>();

    private IList<IMetaIdentifiableObject> metaObjects;

    private Dictionary<Guid, IMetaIdentifiableObject> metaIdentifiableObjectById;
    private Dictionary<string, IMetaIdentifiableObject> metaIdentifiableObjectByTag;
    private Dictionary<string, Composite> compositeByLowercaseName;

    private string[] derivedWorkspaceNames;

    private bool initialized;

    protected MetaPopulation()
    {
        this.initialized = false;

        this.metaObjects = new List<IMetaIdentifiableObject>();
    }

    public bool IsBound { get; private set; }

    public IReadOnlyList<string> WorkspaceNames => this.derivedWorkspaceNames;

    IReadOnlyList<IDomain> IMetaPopulation.Domains => this.Domains;

    public Domain[] Domains { get; set; }

    IReadOnlyList<IClass> IMetaPopulation.Classes => this.Classes;

    public Class[] Classes { get; set; }

    IReadOnlyList<IRelationType> IMetaPopulation.RelationTypes => this.RelationTypes;

    public RelationType[] RelationTypes { get; set; }

    IReadOnlyList<IInterface> IMetaPopulation.Interfaces => this.Interfaces;

    public Interface[] Interfaces { get; set; }

    IReadOnlyList<IComposite> IMetaPopulation.Composites => this.Composites;

    public Composite[] Composites { get; set; }

    IReadOnlyList<IUnit> IMetaPopulation.Units => this.Units;

    public Unit[] Units { get; set; }

    IReadOnlyList<IMethodType> IMetaPopulation.MethodTypes => this.MethodTypes;

    public MethodType[] MethodTypes { get; set; }

    public bool IsValid
    {
        get
        {
            var validation = this.Validate();
            return !validation.ContainsErrors;
        }
    }

    IMetaIdentifiableObject IMetaPopulation.FindById(Guid id) => this.FindById(id);

    public IMetaIdentifiableObject FindById(Guid id)
    {
        this.metaIdentifiableObjectById.TryGetValue(id, out var metaObject);

        return metaObject;
    }

    IMetaIdentifiableObject IMetaPopulation.FindByTag(string tag) => this.FindByTag(tag);

    public IMetaIdentifiableObject FindByTag(string tag)
    {
        this.metaIdentifiableObjectByTag.TryGetValue(tag, out var metaObject);

        return metaObject;
    }

    IComposite IMetaPopulation.FindCompositeByName(string name) => this.FindCompositeByName(name);

    public Composite FindCompositeByName(string name)
    {
        this.compositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
        return composite;
    }

    IValidationLog IMetaPopulation.Validate() => this.Validate();

    public ValidationLog Validate()
    {
        var log = new ValidationLog();

        foreach (var domain in this.Domains)
        {
            domain.Validate(log);
        }

        foreach (var unitType in this.Units)
        {
            unitType.Validate(log);
        }

        foreach (var @interface in this.Interfaces)
        {
            @interface.Validate(log);
        }

        foreach (var @class in this.Classes)
        {
            @class.Validate(log);
        }

        foreach (var relationType in this.RelationTypes)
        {
            relationType.Validate(log);
        }

        foreach (var methodType in this.MethodTypes)
        {
            methodType.Validate(log);
        }

        return log;
    }

    public void Initialize()
    {
        if (this.initialized)
        {
            throw new Exception("Meta is already initialized");
        }

        this.initialized = true;

        this.metaIdentifiableObjectById = this.metaObjects.OfType<IMetaIdentifiableObject>().ToDictionary(v => v.Id, v => v);

        this.Domains = this.metaObjects.OfType<Domain>().ToArray();
        this.Units = this.metaObjects.OfType<Unit>().ToArray();
        this.Interfaces = this.metaObjects.OfType<Interface>().ToArray();
        this.Classes = this.metaObjects.OfType<Class>().ToArray();
        this.RelationTypes = this.metaObjects.OfType<RelationType>().ToArray();
        this.MethodTypes = this.metaObjects.OfType<MethodType>().ToArray();

        this.Composites = this.Classes.Cast<Composite>().Union(this.Interfaces).ToArray();

        // Domains
        foreach (var domain in this.Domains)
        {
            domain.InitializeSuperdomains();
        }

        // DirectSubtypes
        foreach (var type in this.Interfaces)
        {
            type.InitializeDirectSubtypes();
        }

        // Supertypes
        foreach (var type in this.Composites)
        {
            type.InitializeSupertypes();
        }

        // Subtypes
        foreach (var type in this.Interfaces)
        {
            type.InitializeSubtypes();
        }

        // Subclasses
        foreach (var type in this.Interfaces)
        {
            type.InitializeSubclasses();
        }

        // Composites
        foreach (var type in this.Interfaces)
        {
            type.InitializeComposites();
        }

        // Exclusive Subclass
        foreach (var type in this.Interfaces)
        {
            type.InitializeExclusiveSubclass();
        }

        // RoleTypes & AssociationTypes
        var roleTypesByAssociationTypeObjectType = this.RelationTypes
            .GroupBy(v => v.AssociationType.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<RoleType>(g.Select(v => v.RoleType)));

        var associationTypesByRoleTypeObjectType = this.RelationTypes
            .GroupBy(v => v.RoleType.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<AssociationType>(g.Select(v => v.AssociationType)));

        // RoleTypes
        foreach (var composite in this.Composites)
        {
            composite.InitializeRoleTypes(roleTypesByAssociationTypeObjectType);
        }

        // AssociationTypes
        foreach (var composite in this.Composites)
        {
            composite.InitializeAssociationTypes(associationTypesByRoleTypeObjectType);
        }

        // MethodTypes
        var methodTypeByClass = this.MethodTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<MethodType>(g));

        foreach (var composite in this.Composites)
        {
            composite.InitializeMethodTypes(methodTypeByClass);
        }

        // Composite RoleTypes
        var compositeRoleTypesByComposite = this.Composites.ToDictionary(v => (IComposite)v, v => new HashSet<ICompositeRoleType>());
        foreach (var relationType in this.RelationTypes)
        {
            relationType.RoleType.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        foreach (var composite in this.Composites)
        {
            composite.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        // Composite MethodTypes
        var compositeMethodTypesByComposite = this.Composites.ToDictionary(v => (IComposite)v, v => new HashSet<ICompositeMethodType>());
        foreach (var methodType in this.MethodTypes)
        {
            methodType.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        foreach (var composite in this.Composites)
        {
            composite.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        this.metaObjects = null;
    }

    public void Derive()
    {
        this.metaIdentifiableObjectByTag = this.metaIdentifiableObjectById.Values.ToDictionary(v => v.Tag, v => v);

        // RoleType
        foreach (var relationType in this.RelationTypes)
        {
            relationType.RoleType.DeriveScaleAndSize();
        }

        // WorkspaceNames
        this.derivedWorkspaceNames = this.Classes.SelectMany(v => v.AssignedWorkspaceNames).Distinct().ToArray();

        foreach (var relationType in this.RelationTypes)
        {
            relationType.DeriveWorkspaceNames();
        }

        foreach (var methodType in this.MethodTypes)
        {
            methodType.DeriveWorkspaceNames();
        }

        foreach (var @interface in this.Interfaces)
        {
            @interface.DeriveWorkspaceNames();
        }

        foreach (var roleType in this.RelationTypes.Select(v => v.RoleType))
        {
            roleType.DeriveIsRequired();
        }

        foreach (var roleType in this.RelationTypes.Select(v => v.RoleType))
        {
            roleType.DeriveIsUnique();
        }

        this.compositeByLowercaseName = this.Composites.ToDictionary(v => v.Name.ToLowerInvariant());
    }

    public void Bind(Type[] types)
    {
        if (!this.IsBound)
        {
            this.IsBound = true;

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var unit in this.Units)
            {
                unit.BoundType = unit.Tag switch
                {
                    UnitTags.Binary => typeof(byte[]),
                    UnitTags.Boolean => typeof(bool),
                    UnitTags.DateTime => typeof(DateTime),
                    UnitTags.Decimal => typeof(decimal),
                    UnitTags.Float => typeof(double),
                    UnitTags.Integer => typeof(int),
                    UnitTags.String => typeof(string),
                    UnitTags.Unique => typeof(Guid),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }

            foreach (var @interface in this.Interfaces)
            {
                @interface.BoundType = typeByName[@interface.Name];
            }

            foreach (var @class in this.Classes)
            {
                @class.BoundType = typeByName[@class.Name];
            }
        }
    }

    internal void OnCreated(IMetaIdentifiableObject metaObject)
    {
        this.metaObjects.Add(metaObject);
    }
}

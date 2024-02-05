// <copyright file="MetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class MetaPopulation : IStaticMetaPopulation
{
    internal static readonly IReadOnlyList<IComposite> EmptyComposites = Array.Empty<IComposite>();
    internal static readonly IReadOnlyList<IDomain> EmptyDomains = Array.Empty<IDomain>();

    private IList<IMetaIdentifiableObject> metaObjects;

    private Dictionary<Guid, IMetaIdentifiableObject> metaIdentifiableObjectById;
    private Dictionary<string, IMetaIdentifiableObject> metaIdentifiableObjectByTag;
    private Dictionary<string, IStaticComposite> compositeByLowercaseName;

    private string[] derivedWorkspaceNames;

    private bool initialized;

    private IReadOnlyList<IStaticDomain> domains;
    private IReadOnlyList<IStaticClass> classes;
    private IReadOnlyList<IStaticRelationType> relationTypes;
    private IReadOnlyList<IStaticInterface> interfaces;
    private IReadOnlyList<IStaticComposite> composites;
    private IReadOnlyList<IStaticUnit> units;
    private IReadOnlyList<IStaticMethodType> methodTypes;

    protected MetaPopulation()
    {
        this.initialized = false;

        this.metaObjects = new List<IMetaIdentifiableObject>();
    }

    public bool IsBound { get; private set; }

    IReadOnlyList<string> IMetaPopulation.WorkspaceNames => this.derivedWorkspaceNames;

    IReadOnlyList<IDomain> IMetaPopulation.Domains => this.domains;

    IReadOnlyList<IStaticDomain> IStaticMetaPopulation.Domains
    {
        get => this.domains;
        set => this.domains = value;
    }

    IReadOnlyList<IClass> IMetaPopulation.Classes => this.classes;

    IReadOnlyList<IStaticClass> IStaticMetaPopulation.Classes
    {
        get => this.classes;
        set => this.classes = value;
    }

    IReadOnlyList<IRelationType> IMetaPopulation.RelationTypes => this.relationTypes;

    IReadOnlyList<IStaticRelationType> IStaticMetaPopulation.RelationTypes
    {
        get => this.relationTypes;
        set => this.relationTypes = value;
    }

    IReadOnlyList<IInterface> IMetaPopulation.Interfaces => this.interfaces;

    IReadOnlyList<IStaticInterface> IStaticMetaPopulation.Interfaces
    {
        get => this.interfaces;
        set => this.interfaces = value;
    }

    IReadOnlyList<IComposite> IMetaPopulation.Composites => this.composites;

    IReadOnlyList<IStaticComposite> IStaticMetaPopulation.Composites
    {
        get => this.composites;
        set => this.composites = value;
    }

    IReadOnlyList<IUnit> IMetaPopulation.Units => this.units;

    IReadOnlyList<IStaticUnit> IStaticMetaPopulation.Units
    {
        get => this.units;
        set => this.units = value;
    }

    IReadOnlyList<IMethodType> IMetaPopulation.MethodTypes => this.methodTypes;

    IReadOnlyList<IStaticMethodType> IStaticMetaPopulation.MethodTypes
    {
        get => this.methodTypes;
        set => this.methodTypes = value;
    }

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

    public IComposite FindCompositeByName(string name)
    {
        this.compositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
        return composite;
    }

    IValidationLog IMetaPopulation.Validate() => this.Validate();

    public ValidationLog Validate()
    {
        var log = new ValidationLog();

        foreach (var domain in this.domains)
        {
            domain.Validate(log);
        }

        foreach (var unitType in this.units)
        {
            unitType.Validate(log);
        }

        foreach (var @interface in this.interfaces)
        {
            @interface.Validate(log);
        }

        foreach (var @class in this.classes)
        {
            @class.Validate(log);
        }

        foreach (var relationType in this.relationTypes)
        {
            relationType.Validate(log);
        }

        foreach (var methodType in this.methodTypes)
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

        this.domains = this.metaObjects.OfType<Domain>().ToArray();
        this.units = this.metaObjects.OfType<Unit>().ToArray();
        this.interfaces = this.metaObjects.OfType<Interface>().ToArray();
        this.classes = this.metaObjects.OfType<Class>().ToArray();
        this.relationTypes = this.metaObjects.OfType<RelationType>().ToArray();
        this.methodTypes = this.metaObjects.OfType<MethodType>().ToArray();

        this.composites = this.classes.Cast<IStaticComposite>().Union(this.interfaces.Cast<IStaticComposite>()).ToArray();

        // Domains
        foreach (var domain in this.domains)
        {
            domain.InitializeSuperdomains();
        }

        // DirectSubtypes
        foreach (var type in this.interfaces)
        {
            type.InitializeDirectSubtypes();
        }

        // Supertypes
        foreach (IStaticComposite type in this.composites)
        {
            type.InitializeSupertypes();
        }

        // Subtypes
        foreach (var type in this.interfaces)
        {
            type.InitializeSubtypes();
        }

        // Subclasses
        foreach (var type in this.interfaces)
        {
            type.InitializeSubclasses();
        }

        // Composites
        foreach (var type in this.interfaces)
        {
            type.InitializeComposites();
        }

        // Exclusive Subclass
        foreach (var type in this.interfaces)
        {
            type.InitializeExclusiveSubclass();
        }

        // RoleTypes & AssociationTypes
        var roleTypesByAssociationTypeObjectType = this.relationTypes
            .GroupBy(v => v.AssociationType.ObjectType)
            .ToDictionary(g => (IStaticComposite)g.Key, g => new HashSet<IRoleType>(g.Select(v => v.RoleType)));

        var associationTypesByRoleTypeObjectType = this.relationTypes
            .GroupBy(v => v.RoleType.ObjectType)
            .ToDictionary(g => (IObjectType)g.Key, g => new HashSet<IAssociationType>(g.Select(v => v.AssociationType)));

        // RoleTypes
        foreach (IStaticComposite composite in this.composites)
        {
            composite.InitializeRoleTypes(roleTypesByAssociationTypeObjectType);
        }

        // AssociationTypes
        foreach (IStaticComposite composite in this.composites)
        {
            composite.InitializeAssociationTypes(associationTypesByRoleTypeObjectType);
        }

        // MethodTypes
        var methodTypeByClass = this.methodTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => (IStaticComposite)g.Key, g => new HashSet<IStaticMethodType>(g));

        foreach (IStaticComposite composite in this.composites)
        {
            composite.InitializeMethodTypes(methodTypeByClass);
        }

        // Composite RoleTypes
        var compositeRoleTypesByComposite = this.composites.ToDictionary(v => (IComposite)v, v => new HashSet<ICompositeRoleType>());
        foreach (var relationType in this.relationTypes)
        {
            relationType.RoleType.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        foreach (IStaticComposite composite in this.composites)
        {
            composite.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        // Composite MethodTypes
        var compositeMethodTypesByComposite = this.composites.ToDictionary(v => (IComposite)v, v => new HashSet<ICompositeMethodType>());
        foreach (var methodType in this.methodTypes)
        {
            methodType.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        foreach (IStaticComposite composite in this.composites)
        {
            composite.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        this.metaObjects = null;
    }

    public void Derive()
    {
        this.metaIdentifiableObjectByTag = this.metaIdentifiableObjectById.Values.ToDictionary(v => v.Tag, v => v);

        // RoleType
        foreach (var relationType in this.relationTypes)
        {
            relationType.RoleType.DeriveScaleAndSize();
        }

        // WorkspaceNames
        this.derivedWorkspaceNames = this.classes.SelectMany(v => v.AssignedWorkspaceNames).Distinct().ToArray();

        foreach (var relationType in this.relationTypes)
        {
            relationType.DeriveWorkspaceNames();
        }

        foreach (var methodType in this.methodTypes)
        {
            methodType.DeriveWorkspaceNames();
        }

        foreach (var @interface in this.interfaces)
        {
            @interface.DeriveWorkspaceNames();
        }

        foreach (var roleType in this.relationTypes.Select(v => v.RoleType))
        {
            roleType.DeriveIsRequired();
        }

        foreach (var roleType in this.relationTypes.Select(v => v.RoleType))
        {
            roleType.DeriveIsUnique();
        }

        foreach (IStaticComposite composite in this.composites)
        {
            composite.DeriveKeyRoleType();
        }

        this.compositeByLowercaseName = this.composites.ToDictionary(v => v.Name.ToLowerInvariant());
    }

    public void Bind(Type[] types)
    {
        if (!this.IsBound)
        {
            this.IsBound = true;

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var unit in this.units)
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

            foreach (var @interface in this.interfaces)
            {
                @interface.BoundType = typeByName[@interface.Name];
            }

            foreach (var @class in this.classes)
            {
                @class.BoundType = typeByName[@class.Name];
            }
        }
    }

    void IStaticMetaPopulation.OnCreated(IMetaIdentifiableObject metaObject)
    {
        this.metaObjects.Add(metaObject);
    }
}

// <copyright file="MetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Embedded.Domain.Memory;
using Embedded;

public sealed class MetaPopulation : EmbeddedPopulation, IEmbeddedPopulation
{
    internal static readonly IReadOnlyList<Composite> EmptyComposites = Array.Empty<Composite>();
    internal static readonly IReadOnlyList<Domain> EmptyDomains = Array.Empty<Domain>();

    private IList<IMetaIdentifiableObject> metaObjects;

    private Dictionary<Guid, IMetaIdentifiableObject> metaIdentifiableObjectById;
    private Dictionary<string, IMetaIdentifiableObject> metaIdentifiableObjectByTag;
    private Dictionary<string, Composite> compositeByLowercaseName;

    private string[] derivedWorkspaceNames;

    private bool initialized;

    private IReadOnlyList<Domain> domains;
    private IReadOnlyList<Class> classes;
    private IReadOnlyList<RoleType> roleTypes;
    private IReadOnlyList<Interface> interfaces;
    private IReadOnlyList<Composite> composites;
    private IReadOnlyList<Unit> units;
    private IReadOnlyList<MethodType> methodTypes;

    public MetaPopulation()
    {
        this.initialized = false;

        this.metaObjects = new List<IMetaIdentifiableObject>();

        this.EmbeddedRoleTypes = new EmbeddedRoleTypes(this.EmbeddedMeta);

        this.RegisterDerivations();
    }

    public EmbeddedRoleTypes EmbeddedRoleTypes { get; set; }

    public bool IsBound { get; private set; }

    public IReadOnlyList<string> WorkspaceNames => this.derivedWorkspaceNames;

    public IReadOnlyList<Domain> Domains
    {
        get => this.domains;
        set => this.domains = value;
    }

    public IReadOnlyList<Class> Classes
    {
        get => this.classes;
        set => this.classes = value;
    }

    public IReadOnlyList<RoleType> RoleTypes
    {
        get => this.roleTypes;
        set => this.roleTypes = value;
    }

    public IReadOnlyList<Interface> Interfaces
    {
        get => this.interfaces;
        set => this.interfaces = value;
    }

    public IReadOnlyList<Composite> Composites
    {
        get => this.composites;
        set => this.composites = value;
    }

    public IReadOnlyList<Unit> Units
    {
        get => this.units;
        set => this.units = value;
    }

    public IReadOnlyList<MethodType> MethodTypes
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

    public IMetaIdentifiableObject FindById(Guid id)
    {
        this.metaIdentifiableObjectById.TryGetValue(id, out var metaObject);

        return metaObject;
    }

    public IMetaIdentifiableObject FindByTag(string tag)
    {
        this.metaIdentifiableObjectByTag.TryGetValue(tag, out var metaObject);

        return metaObject;
    }

    public Composite FindCompositeByName(string name)
    {
        this.compositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
        return composite;
    }

    public IValidationLog Validate()
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

        foreach (var relationType in this.roleTypes)
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
        this.roleTypes = this.metaObjects.OfType<RoleType>().ToArray();
        this.methodTypes = this.metaObjects.OfType<MethodType>().ToArray();

        this.composites = this.classes.Union(this.interfaces.Cast<Composite>()).ToArray();

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
        foreach (Composite type in this.composites)
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
        var roleTypesByAssociationTypeObjectType = this.roleTypes
            .GroupBy(v => v.AssociationType.ObjectType)
            .ToDictionary(g => (Composite)g.Key, g => new HashSet<RoleType>(g));

        var associationTypesByRoleTypeObjectType = this.roleTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<AssociationType>(g.Select(v => v.AssociationType)));

        // RoleTypes
        foreach (Composite composite in this.composites)
        {
            composite.InitializeRoleTypes(roleTypesByAssociationTypeObjectType);
        }

        // AssociationTypes
        foreach (Composite composite in this.composites)
        {
            composite.InitializeAssociationTypes(associationTypesByRoleTypeObjectType);
        }

        // MethodTypes
        var methodTypeByClass = this.methodTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<MethodType>(g));

        foreach (Composite composite in this.composites)
        {
            composite.InitializeMethodTypes(methodTypeByClass);
        }

        // Composite RoleTypes
        var compositeRoleTypesByComposite = this.composites.ToDictionary(v => v, v => new HashSet<CompositeRoleType>());
        foreach (var relationType in this.roleTypes)
        {
            relationType.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        foreach (Composite composite in this.composites)
        {
            composite.InitializeCompositeRoleTypes(compositeRoleTypesByComposite);
        }

        // Composite MethodTypes
        var compositeMethodTypesByComposite = this.composites.ToDictionary(v => v, v => new HashSet<CompositeMethodType>());
        foreach (var methodType in this.methodTypes)
        {
            methodType.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        foreach (Composite composite in this.composites)
        {
            composite.InitializeCompositeMethodTypes(compositeMethodTypesByComposite);
        }

        this.metaObjects = null;
    }

    public void Derive()
    {
        this.metaIdentifiableObjectByTag = this.metaIdentifiableObjectById.Values.ToDictionary(v => v.Tag, v => v);

        // RoleType
        foreach (var relationType in this.roleTypes)
        {
            relationType.DeriveScaleAndSize();
        }

        // WorkspaceNames
        this.derivedWorkspaceNames = this.classes.SelectMany(v => v.AssignedWorkspaceNames).Distinct().ToArray();

        foreach (var relationType in this.roleTypes)
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

        foreach (var roleType in this.roleTypes)
        {
            roleType.DeriveIsRequired();
        }

        foreach (var roleType in this.roleTypes)
        {
            roleType.DeriveIsUnique();
        }

        foreach (Composite composite in this.composites)
        {
            composite.DeriveKeyRoleType();
        }

        this.compositeByLowercaseName = this.composites.ToDictionary(v => v.SingularName.ToLowerInvariant());

        this.EmbeddedDerive();
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
                @interface.BoundType = typeByName[@interface.SingularName];
            }

            foreach (var @class in this.classes)
            {
                @class.BoundType = typeByName[@class.SingularName];
            }
        }
    }

    public T Create<T>(params Action<T>[] builders) where T : IEmbeddedObject
    {
        return this.EmbeddedCreateObject(builders);
    }

    public void OnCreated(IMetaIdentifiableObject metaObject)
    {
        this.metaObjects.Add(metaObject);
    }
}

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

public abstract class MetaPopulation : IMetaPopulation
{
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

    public MethodCompiler MethodCompiler { get; private set; }

    public IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    IDomain[] IMetaPopulation.Domains => this.Domains;

    public Domain[] Domains { get; set; }

    IClass[] IMetaPopulation.Classes => this.Classes;

    public Class[] Classes { get; set; }

    IRelationType[] IMetaPopulation.RelationTypes => this.RelationTypes;

    public RelationType[] RelationTypes { get; set; }

    IInterface[] IMetaPopulation.Interfaces => this.Interfaces;

    public Interface[] Interfaces { get; set; }

    IComposite[] IMetaPopulation.Composites => this.Composites;

    public Composite[] Composites { get; set; }

    IUnit[] IMetaPopulation.Units => this.Units;

    public Unit[] Units { get; set; }

    IMethodType[] IMetaPopulation.MethodTypes => this.MethodTypes;

    public MethodType[] MethodTypes { get; set; }

    IRecord[] IMetaPopulation.Records => this.Records;

    public Record[] Records { get; set; }

    IFieldType[] IMetaPopulation.FieldTypes => this.FieldTypes;

    public FieldType[] FieldTypes { get; set; }

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

        foreach (var record in this.Records)
        {
            record.Validate(log);
        }

        foreach (var fieldType in this.FieldTypes)
        {
            fieldType.Validate(log);
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
        this.Records = this.metaObjects.OfType<Record>().ToArray();
        this.FieldTypes = this.metaObjects.OfType<FieldType>().ToArray();

        this.Composites = this.Classes.Cast<Composite>().Union(this.Interfaces).ToArray();

        var sharedDomains = new HashSet<Domain>();
        var sharedComposites = new HashSet<Composite>();
        var sharedInterfaces = new HashSet<Interface>();
        var sharedClasses = new HashSet<Class>();
        var sharedAssociationTypes = new HashSet<AssociationType>();
        var sharedRoleTypes = new HashSet<RoleType>();
        var sharedMethodTypeList = new HashSet<MethodType>();

        // Domains
        foreach (var domain in this.Domains)
        {
            domain.InitializeSuperdomains(sharedDomains);
        }

        // DirectSubtypes
        foreach (var type in this.Interfaces)
        {
            type.InitializeDirectSubtypes();
        }

        // Supertypes
        foreach (var type in this.Composites)
        {
            type.InitializeSupertypes(sharedInterfaces);
        }

        // Subtypes
        foreach (var type in this.Interfaces)
        {
            type.InitializeSubtypes(sharedComposites);
        }

        // Subclasses
        foreach (var type in this.Interfaces)
        {
            type.InitializeSubclasses(sharedClasses);
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
            composite.InitializeRoleTypes(sharedRoleTypes, roleTypesByAssociationTypeObjectType);
        }

        // AssociationTypes
        foreach (var composite in this.Composites)
        {
            composite.InitializeAssociationTypes(sharedAssociationTypes, associationTypesByRoleTypeObjectType);
        }

        // MethodTypes
        var methodTypeByClass = this.MethodTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<MethodType>(g));

        foreach (var composite in this.Composites)
        {
            composite.InitializeMethodTypes(sharedMethodTypeList, methodTypeByClass);
        }

        // Records
        var fieldTypesByRecord = this.FieldTypes
            .GroupBy(v => v.Record)
            .ToDictionary(g => g.Key, g => g.ToArray());

        foreach (var record in this.Records)
        {
            record.InitializeFieldTypes(fieldTypesByRecord);
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

        // Required RoleTypes
        foreach (var @class in this.Classes)
        {
            @class.DeriveRequiredRoleTypes();
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

        IDictionary<Record, ISet<string>> workspaceNamesByRecord = new Dictionary<Record, ISet<string>>();

        foreach (var methodType in this.MethodTypes)
        {
            methodType.PrepareWorkspaceNames(workspaceNamesByRecord);
        }

        foreach (var record in this.Records)
        {
            record.DeriveWorkspaceNames(workspaceNamesByRecord);
        }

        foreach (var fieldType in this.FieldTypes)
        {
            fieldType.DeriveWorkspaceNames();
        }

        this.compositeByLowercaseName = this.Composites.ToDictionary(v => v.Name.ToLowerInvariant());
    }

    public void Bind(Type[] types, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface)
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

            foreach (var record in this.Records)
            {
                record.Bind(typeByName);
            }

            this.MethodCompiler = new MethodCompiler(this, extensionMethodsByInterface);
        }
    }

    internal void OnCreated(IMetaIdentifiableObject metaObject)
    {
        this.metaObjects.Add(metaObject);
    }
}

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
    private IList<IMetaObject> metaObjects;

    private Dictionary<Guid, IMetaIdentifiableObject> metaIdentifiableObjectById;
    private Dictionary<string, IMetaIdentifiableObject> metaIdentifiableObjectByTag;
    private Dictionary<string, Composite> compositeByLowercaseName;

    private string[] derivedWorkspaceNames;

    protected MetaPopulation()
    {
        this.metaObjects = new List<IMetaObject>();
    }

    public bool IsBound { get; private set; }

    public MethodCompiler MethodCompiler { get; private set; }

    public IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    IDomain[] IMetaPopulation.Domains => this.Domains;

    public Domain[] Domains { get; set; }

    IClass[] IMetaPopulation.Classes => this.Classes;

    public Class[] Classes { get; set; }

    public Inheritance[] Inheritances { get; set; }

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

    IMetaIdentifiableObject IMetaPopulation.FindByTag(string tag) => this.FindByTag(tag);

    IComposite IMetaPopulation.FindDatabaseCompositeByName(string name) => this.FindDatabaseCompositeByName(name);

    IValidationLog IMetaPopulation.Validate() => this.Validate();

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

    public Composite FindDatabaseCompositeByName(string name)
    {
        this.compositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
        return composite;
    }

    public void StructuralDerive()
    {
        this.metaIdentifiableObjectById = this.metaObjects.OfType<IMetaIdentifiableObject>().ToDictionary(v => v.Id, v => v);

        this.Domains = this.metaObjects.OfType<Domain>().ToArray();
        this.Units = this.metaObjects.OfType<Unit>().ToArray();
        this.Interfaces = this.metaObjects.OfType<Interface>().ToArray();
        this.Classes = this.metaObjects.OfType<Class>().ToArray();
        this.Inheritances = this.metaObjects.OfType<Inheritance>().ToArray();
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
            domain.StructuralDeriveSuperdomains(sharedDomains);
        }

        // DirectSupertypes
        foreach (var type in this.Composites)
        {
            type.StructuralDeriveDirectSupertypes(sharedInterfaces);
        }

        // DirectSubtypes
        foreach (var type in this.Interfaces)
        {
            type.StructuralDeriveDirectSubtypes(sharedComposites);
        }

        // Supertypes
        foreach (var type in this.Composites)
        {
            type.StructuralDeriveSupertypes(sharedInterfaces);
        }

        // Subtypes
        foreach (var type in this.Interfaces)
        {
            type.StructuralDeriveSubtypes(sharedComposites);
        }

        // Subclasses
        foreach (var type in this.Interfaces)
        {
            type.StructuralDeriveSubclasses(sharedClasses);
        }

        // Exclusive Subclass
        foreach (var type in this.Interfaces)
        {
            type.StructuralDeriveExclusiveSubclass();
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
            composite.StructuralDeriveRoleTypes(sharedRoleTypes, roleTypesByAssociationTypeObjectType);
        }

        // AssociationTypes
        foreach (var composite in this.Composites)
        {
            composite.StructuralDeriveAssociationTypes(sharedAssociationTypes, associationTypesByRoleTypeObjectType);
        }

        // MethodTypes
        var methodTypeByClass = this.MethodTypes
            .GroupBy(v => v.ObjectType)
            .ToDictionary(g => g.Key, g => new HashSet<MethodType>(g));

        foreach (var composite in this.Composites)
        {
            composite.StructuralDeriveMethodTypes(sharedMethodTypeList, methodTypeByClass);
        }

        // Records
        var fieldTypesByRecord = this.FieldTypes
            .GroupBy(v => v.Record)
            .ToDictionary(g => g.Key, g => g.ToArray());

        foreach (var record in this.Records)
        {
            record.StructuralDeriveFieldTypes(fieldTypesByRecord);
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
        var workspaceNames = new HashSet<string>();
        foreach (var @class in this.Classes)
        {
            @class.DeriveWorkspaceNames(workspaceNames);
        }

        this.derivedWorkspaceNames = workspaceNames.ToArray();

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

        foreach (var domain in this.Domains)
        {
            domain.DeriveWorkspaceNames();
        }

        this.compositeByLowercaseName = this.Composites.ToDictionary(v => v.Name.ToLowerInvariant());
    }

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

        foreach (var inheritance in this.Inheritances)
        {
            inheritance.Validate(log);
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

        var inheritancesBySubtype = new Dictionary<Composite, List<Inheritance>>();
        foreach (var inheritance in this.Inheritances)
        {
            var subtype = inheritance.Subtype;
            if (subtype != null)
            {
                if (!inheritancesBySubtype.TryGetValue(subtype, out var inheritanceList))
                {
                    inheritanceList = new List<Inheritance>();
                    inheritancesBySubtype[subtype] = inheritanceList;
                }

                inheritanceList.Add(inheritance);
            }
        }

        var supertypes = new HashSet<Interface>();
        foreach (var subtype in inheritancesBySubtype.Keys)
        {
            supertypes.Clear();
            if (this.HasCycle(subtype, supertypes, inheritancesBySubtype))
            {
                var message = subtype.ValidationName + " has a cycle in its inheritance hierarchy";
                log.AddError(message, subtype, ValidationKind.Cyclic, "IComposite.Supertypes");
            }
        }

        return log;
    }

    public void Bind(Type[] types, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface)
    {
        if (!this.IsBound)
        {
            this.IsBound = true;

            foreach (var domain in this.Domains)
            {
                domain.Bind();
            }

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

    internal void OnCreated(IMetaObject metaObject)
    {
        this.metaObjects.Add(metaObject);
    }

    private bool HasCycle(Composite subtype, HashSet<Interface> supertypes, Dictionary<Composite, List<Inheritance>> inheritancesBySubtype)
    {
        foreach (var inheritance in inheritancesBySubtype[subtype])
        {
            var supertype = inheritance.Supertype;
            if (supertype != null && this.HasCycle(subtype, supertype, supertypes, inheritancesBySubtype))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasCycle(Composite originalSubtype, Interface currentSupertype, HashSet<Interface> supertypes, Dictionary<Composite, List<Inheritance>> inheritancesBySubtype)
    {
        if (originalSubtype is Interface @interface && supertypes.Contains(@interface))
        {
            return true;
        }

        if (!supertypes.Contains(currentSupertype))
        {
            supertypes.Add(currentSupertype);

            if (inheritancesBySubtype.TryGetValue(currentSupertype, out var currentSuperInheritances))
            {
                foreach (var inheritance in currentSuperInheritances)
                {
                    var newSupertype = inheritance.Supertype;
                    if (newSupertype != null && this.HasCycle(originalSubtype, newSupertype, supertypes, inheritancesBySubtype))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

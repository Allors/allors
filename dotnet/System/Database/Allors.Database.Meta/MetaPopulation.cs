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
using Microsoft.VisualBasic.FileIO;

public abstract class MetaPopulation : IMetaPopulation
{
    private readonly Dictionary<Guid, IMetaIdentifiableObject> metaObjectById;
    private readonly Dictionary<string, IMetaIdentifiableObject> metaObjectByTag;
    private IList<Class> classes;
    private Dictionary<string, Composite> derivedCompositeByLowercaseName;

    private string[] derivedWorkspaceNames;

    private IList<Domain> domains;
    private IList<FieldType> fieldTypes;
    private IList<Inheritance> inheritances;
    private IList<Interface> interfaces;
    private bool isDeriving;

    private bool isStale;
    private bool isStructuralDeriving;
    private IList<MethodType> methodTypes;
    private IList<Record> records;
    private IList<RelationType> relationTypes;

    private Composite[] structuralDerivedComposites;
    private IList<Unit> units;

    protected MetaPopulation()
    {
        this.isStale = true;
        this.isDeriving = false;

        this.domains = new List<Domain>();
        this.units = new List<Unit>();
        this.interfaces = new List<Interface>();
        this.classes = new List<Class>();
        this.inheritances = new List<Inheritance>();
        this.relationTypes = new List<RelationType>();
        this.methodTypes = new List<MethodType>();
        this.records = new List<Record>();
        this.fieldTypes = new List<FieldType>();

        this.metaObjectById = new Dictionary<Guid, IMetaIdentifiableObject>();
        this.metaObjectByTag = new Dictionary<string, IMetaIdentifiableObject>();
    }

    public MethodCompiler MethodCompiler { get; private set; }

    public IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    private bool IsBound { get; set; }
    public IEnumerable<Domain> Domains => this.domains;
    public IEnumerable<Class> Classes => this.classes;
    public IEnumerable<Inheritance> Inheritances => this.inheritances;
    public IEnumerable<RelationType> RelationTypes => this.relationTypes;
    public IEnumerable<Interface> Interfaces => this.interfaces;
    public IEnumerable<Composite> Composites => this.structuralDerivedComposites;
    public IEnumerable<Unit> Units => this.units;
    public IEnumerable<Record> Records => this.records;
    public IEnumerable<MethodType> MethodTypes => this.methodTypes;

    IEnumerable<IDomain> IMetaPopulation.Domains => this.Domains;

    IEnumerable<IClass> IMetaPopulation.Classes => this.classes;

    IEnumerable<IRelationType> IMetaPopulation.RelationTypes => this.relationTypes;

    IEnumerable<IInterface> IMetaPopulation.Interfaces => this.interfaces;

    IEnumerable<IComposite> IMetaPopulation.Composites => this.Composites;

    public bool IsValid
    {
        get
        {
            var validation = this.Validate();
            return !validation.ContainsErrors;
        }
    }

    IEnumerable<IUnit> IMetaPopulation.Units => this.Units;

    IEnumerable<IMethodType> IMetaPopulation.MethodTypes => this.MethodTypes;

    IMetaIdentifiableObject IMetaPopulation.FindById(Guid id) => this.FindById(id);

    IMetaIdentifiableObject IMetaPopulation.FindByTag(string tag) => this.FindByTag(tag);

    IComposite IMetaPopulation.FindDatabaseCompositeByName(string name) => this.FindDatabaseCompositeByName(name);

    IValidationLog IMetaPopulation.Validate() => this.Validate();

    public void Bind(Type[] types, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface)
    {
        if (!this.IsBound)
        {
            this.Derive();

            this.IsBound = true;

            foreach (var domain in this.domains)
            {
                domain.Bind();
            }

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var unit in this.units)
            {
                unit.Bind();
            }

            foreach (var @interface in this.Interfaces)
            {
                @interface.Bind(typeByName);
            }

            foreach (var @class in this.Classes)
            {
                @class.Bind(typeByName);
            }

            foreach (var record in this.records)
            {
                record.Bind(typeByName);
            }

            this.MethodCompiler = new MethodCompiler(this, extensionMethodsByInterface);
        }
    }

    public IMetaIdentifiableObject FindById(Guid id)
    {
        this.metaObjectById.TryGetValue(id, out var metaObject);

        return metaObject;
    }

    public IMetaIdentifiableObject FindByTag(string tag)
    {
        this.metaObjectByTag.TryGetValue(tag, out var metaObject);

        return metaObject;
    }

    public Composite FindDatabaseCompositeByName(string name)
    {
        this.Derive();

        this.derivedCompositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);

        return composite;
    }

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

    public void AssertUnlocked()
    {
        if (this.IsBound)
        {
            throw new Exception("Environment is locked");
        }
    }

    public void StructuralDerive()
    {
        this.isStructuralDeriving = true;

        try
        {
            this.domains = this.domains.ToArray();
            this.units = this.units.ToArray();
            this.interfaces = this.interfaces.ToArray();
            this.classes = this.classes.ToArray();
            this.inheritances = this.inheritances.ToArray();
            this.relationTypes = this.relationTypes.ToArray();
            this.methodTypes = this.methodTypes.ToArray();
            this.records = this.records.ToArray();
            this.fieldTypes = this.fieldTypes.ToArray();

            var sharedDomains = new HashSet<Domain>();
            var sharedComposites = new HashSet<Composite>();
            var sharedInterfaces = new HashSet<Interface>();
            var sharedClasses = new HashSet<Class>();
            var sharedAssociationTypes = new HashSet<AssociationType>();
            var sharedRoleTypes = new HashSet<RoleType>();
            var sharedMethodTypeList = new HashSet<MethodType>();

            // Domains
            foreach (var domain in this.domains)
            {
                domain.StructuralDeriveSuperdomains(sharedDomains);
            }

            // Unit & IComposite ObjectTypes
            var compositeTypes = new List<Composite>(this.interfaces);
            compositeTypes.AddRange(this.Classes);
            this.structuralDerivedComposites = compositeTypes.ToArray();

            // DirectSupertypes
            foreach (var type in this.Composites)
            {
                type.StructuralDeriveDirectSupertypes(sharedInterfaces);
            }

            // DirectSubtypes
            foreach (var type in this.interfaces)
            {
                type.StructuralDeriveDirectSubtypes(sharedComposites);
            }

            // Supertypes
            foreach (var type in this.Composites)
            {
                type.StructuralDeriveSupertypes(sharedInterfaces);
            }

            // Subtypes
            foreach (var type in this.interfaces)
            {
                type.StructuralDeriveSubtypes(sharedComposites);
            }

            // Subclasses
            foreach (var type in this.interfaces)
            {
                type.StructuralDeriveSubclasses(sharedClasses);
            }

            // Exclusive Subclass
            foreach (var type in this.interfaces)
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
        }
        finally
        {
            this.isStructuralDeriving = false;
        }
    }

    public void Derive()
    {
        if (this.isStructuralDeriving)
        {
            throw new Exception("Structural Derive is ongoing");
        }

        if (this.isStale && !this.isDeriving)
        {
            try
            {
                this.isDeriving = true;

                // RoleType
                foreach (var relationType in this.RelationTypes)
                {
                    relationType.RoleType.DeriveScaleAndSize();
                }

                // RelationType Multiplicity
                foreach (var relationType in this.RelationTypes)
                {
                    relationType.DeriveMultiplicity();
                }

                // Required RoleTypes
                foreach (var @class in this.classes)
                {
                    @class.DeriveRequiredRoleTypes();
                }

                // WorkspaceNames
                var workspaceNames = new HashSet<string>();
                foreach (var @class in this.classes)
                {
                    @class.DeriveWorkspaceNames(workspaceNames);
                }

                this.derivedWorkspaceNames = workspaceNames.ToArray();

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

                IDictionary<Record, ISet<string>> workspaceNamesByRecord = new Dictionary<Record, ISet<string>>();

                foreach (var methodType in this.methodTypes)
                {
                    methodType.PrepareWorkspaceNames(workspaceNamesByRecord);
                }

                foreach (var record in this.records)
                {
                    record.DeriveWorkspaceNames(workspaceNamesByRecord);
                }

                foreach (var fieldType in this.fieldTypes)
                {
                    fieldType.DeriveWorkspaceNames();
                }

                foreach (var domain in this.domains)
                {
                    domain.DeriveWorkspaceNames();
                }

                // MetaPopulation
                this.derivedCompositeByLowercaseName = this.Composites.ToDictionary(v => v.Name.ToLowerInvariant());
            }
            finally
            {
                // Ignore stale requests during a derivation
                this.isStale = false;
                this.isDeriving = false;
            }
        }
    }

    internal void OnDomainCreated(Domain domain)
    {
        this.domains.Add(domain);
        this.metaObjectById.Add(domain.Id, domain);
        this.metaObjectByTag.Add(domain.Tag, domain);

        this.Stale();
    }

    public void OnFieldTypeCreated(FieldType fieldType)
    {
        this.fieldTypes.Add(fieldType);
        this.metaObjectById.Add(fieldType.Id, fieldType);
        this.metaObjectByTag.Add(fieldType.Tag, fieldType);

        this.Stale();
    }

    internal void OnUnitCreated(Unit unit)
    {
        this.units.Add(unit);
        this.metaObjectById.Add(unit.Id, unit);
        this.metaObjectByTag.Add(unit.Tag, unit);

        this.Stale();
    }

    internal void OnInterfaceCreated(Interface @interface)
    {
        this.interfaces.Add(@interface);
        this.metaObjectById.Add(@interface.Id, @interface);
        this.metaObjectByTag.Add(@interface.Tag, @interface);

        this.Stale();
    }

    internal void OnClassCreated(Class @class)
    {
        this.classes.Add(@class);
        this.metaObjectById.Add(@class.Id, @class);
        this.metaObjectByTag.Add(@class.Tag, @class);

        this.Stale();
    }

    internal void OnInheritanceCreated(Inheritance inheritance)
    {
        this.inheritances.Add(inheritance);
        this.Stale();
    }

    internal void OnRelationTypeCreated(RelationType relationType)
    {
        this.relationTypes.Add(relationType);
        this.metaObjectById.Add(relationType.Id, relationType);
        this.metaObjectByTag.Add(relationType.Tag, relationType);

        this.Stale();
    }

    internal void OnAssociationTypeCreated(AssociationType associationType) => this.Stale();

    internal void OnRecordCreated(Record record)
    {
        this.records.Add(record);
        this.metaObjectById.Add(record.Id, record);
        this.metaObjectByTag.Add(record.Tag, record);

        this.Stale();
    }

    internal void OnRoleTypeCreated(RoleType roleType) => this.Stale();

    internal void OnMethodTypeCreated(MethodType methodType)
    {
        this.methodTypes.Add(methodType);
        this.metaObjectById.Add(methodType.Id, methodType);
        this.metaObjectByTag.Add(methodType.Tag, methodType);

        this.Stale();
    }

    internal void Stale() => this.isStale = true;

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

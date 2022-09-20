// <copyright file="Interface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Interface : Composite, IInterface
{
    private string[] derivedWorkspaceNames;
    private HashSet<Class> structuralDerivedClasses;

    private HashSet<Composite> structuralDerivedDirectSubtypes;
    private Class structuralDerivedExclusiveClass;
    private HashSet<Composite> structuralDerivedSubtypes;

    protected Interface(MetaPopulation metaPopulation, Guid id)
        : base(metaPopulation, id) =>
        metaPopulation.OnInterfaceCreated(this);

    public bool ExistClasses => this.structuralDerivedClasses.Count > 0;

    public bool ExistSubtypes => this.structuralDerivedSubtypes.Count > 0;

    public override IEnumerable<Class> Classes => this.structuralDerivedClasses;
    public override IEnumerable<Composite> Subtypes => this.structuralDerivedSubtypes;

    public IEnumerable<Interface> Subinterfaces => this.Subtypes.OfType<Interface>();

    public override Class ExclusiveClass => this.structuralDerivedExclusiveClass;

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    public override bool ExistClass => this.structuralDerivedClasses.Count > 0;

    IEnumerable<IComposite> IComposite.Subtypes => this.Subtypes;

    public override bool IsAssignableFrom(IComposite objectType) =>
        this.Equals(objectType) || this.structuralDerivedSubtypes.Contains(objectType);



    internal void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(this.AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    internal void StructuralDeriveDirectSubtypes(HashSet<Composite> directSubtypes)
    {
        directSubtypes.Clear();
        foreach (var inheritance in this.MetaPopulation.Inheritances.Where(inheritance => this.Equals(inheritance.Supertype)))
        {
            directSubtypes.Add(inheritance.Subtype);
        }

        this.structuralDerivedDirectSubtypes = new HashSet<Composite>(directSubtypes);
    }

    internal void StructuralDeriveSubclasses(HashSet<Class> subClasses)
    {
        subClasses.Clear();
        foreach (var subType in this.structuralDerivedSubtypes)
        {
            if (subType is IClass)
            {
                subClasses.Add((Class)subType);
            }
        }

        this.structuralDerivedClasses = new HashSet<Class>(subClasses);
    }

    internal void StructuralDeriveSubtypes(HashSet<Composite> subTypes)
    {
        subTypes.Clear();
        this.StructuralDeriveSubtypesRecursively(this, subTypes);

        this.structuralDerivedSubtypes = new HashSet<Composite>(subTypes);
    }

    internal void StructuralDeriveExclusiveSubclass() => this.structuralDerivedExclusiveClass =
        this.structuralDerivedClasses.Count == 1 ? this.structuralDerivedClasses.First() : null;

    internal void StructuralDeriveSubtypesRecursively(ObjectType type, HashSet<Composite> subTypes)
    {
        foreach (var directSubtype in this.structuralDerivedDirectSubtypes)
        {
            if (!Equals(directSubtype, type))
            {
                subTypes.Add(directSubtype);
                if (directSubtype is IInterface)
                {
                    ((Interface)directSubtype).StructuralDeriveSubtypesRecursively(this, subTypes);
                }
            }
        }
    }
}

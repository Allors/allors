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

    private IReadOnlySet<IComposite> composites;
    private IReadOnlySet<IComposite> subtypes;
    private IReadOnlySet<IClass> subclasses;
    private IClass exclusiveClass;

    protected Interface(MetaPopulation metaPopulation, Guid id, Interface[] directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, directSupertypes, singularName, assignedPluralName) =>
        metaPopulation.OnCreated(this);

    public IReadOnlySet<IComposite> DirectSubtypes { get; private set; }

    public override IReadOnlySet<IComposite> Composites => this.composites;

    public override IReadOnlySet<IClass> Classes => this.subclasses;

    public override IReadOnlySet<IComposite> Subtypes => this.subtypes;

    public override IClass ExclusiveClass => this.exclusiveClass;

    public override IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    public override bool IsAssignableFrom(IComposite objectType) =>
        this.Equals(objectType) || this.subtypes.Contains(objectType);

    internal void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(this.AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    internal void InitializeDirectSubtypes()
    {
        this.DirectSubtypes = new HashSet<IComposite>(this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)));
    }

    internal void InitializeSubtypes()
    {
        var subtypes = new HashSet<IComposite>();
        this.InitializeSubtypesRecursively(this, subtypes);
        this.subtypes = new HashSet<IComposite>(subtypes);
    }

    internal void InitializeSubclasses()
    {
        var subclasses = new HashSet<Class>();
        foreach (var subType in this.subtypes.OfType<IClass>())
        {
            subclasses.Add((Class)subType);
        }

        this.subclasses = new HashSet<IClass>(subclasses);
    }

    internal void InitializeComposites()
    {
        this.composites = new HashSet<IComposite>(this.subtypes.Append(this));
    }

    internal void InitializeExclusiveSubclass() => this.exclusiveClass = this.subclasses.Count == 1 ? this.subclasses.First() : null;

    private void InitializeSubtypesRecursively(ObjectType type, ISet<IComposite> subtypes)
    {
        foreach (var directSubtype in this.DirectSubtypes.Cast<Composite>())
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

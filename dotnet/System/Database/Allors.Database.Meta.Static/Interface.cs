// <copyright file="Interface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

    private IReadOnlyList<IComposite> composites;
    private IReadOnlyList<IComposite> directSubtypes;
    private IReadOnlyList<IComposite> subtypes;
    private IReadOnlyList<IClass> subclasses;
    private IClass exclusiveClass;

    protected Interface(MetaPopulation metaPopulation, Guid id, IReadOnlyList<Interface> directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, directSupertypes, singularName, assignedPluralName) =>
        metaPopulation.OnCreated(this);

    public override IReadOnlyList<IComposite> DirectSubtypes => this.directSubtypes;

    public override IReadOnlyList<IComposite> Composites => this.composites;

    public override IReadOnlyList<IClass> Classes => this.subclasses;

    public override IReadOnlyList<IComposite> Subtypes => this.subtypes;

    public override IClass ExclusiveClass => this.exclusiveClass;

    public override IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    public override bool IsAssignableFrom(IComposite objectType) =>
        this.Equals(objectType) || this.subtypes.Contains(objectType);

    internal void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = ((IInterface)this)
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(((IInterface)this).AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    internal void InitializeDirectSubtypes()
    {
        this.directSubtypes = this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)).ToArray();
    }

    internal void InitializeSubtypes()
    {
        var subtypes = new HashSet<IComposite>();
        this.InitializeSubtypesRecursively(this, subtypes);
        this.subtypes = subtypes.ToArray();
    }

    internal void InitializeSubclasses()
    {
        var subclasses = new HashSet<Class>();
        foreach (var subType in this.subtypes.OfType<IClass>())
        {
            subclasses.Add((Class)subType);
        }

        this.subclasses = subclasses.ToArray();
    }

    internal void InitializeComposites()
    {
        this.composites = this.subtypes.Append(this).ToArray();
    }

    internal void InitializeExclusiveSubclass() => this.exclusiveClass = this.subclasses.Count == 1 ? this.subclasses.First() : null;

    private void InitializeSubtypesRecursively(IObjectType type, ISet<IComposite> subtypes)
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

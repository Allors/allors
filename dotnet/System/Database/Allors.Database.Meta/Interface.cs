// <copyright file="Interface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Embedded;
using Allors.Embedded.Meta;
using Text;

public sealed class Interface : Composite
{
    private string[] derivedWorkspaceNames;

    private IReadOnlyList<Composite> composites;
    private IReadOnlyList<Composite> directSubtypes;
    private IReadOnlyList<Composite> subtypes;
    private IReadOnlyList<Class> subclasses;
    private Class exclusiveClass;

    public Interface(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        metaPopulation.OnCreated(this);
    }
    
    private RoleType derivedKeyRoleType;
    
    public override bool IsInterface => true;

    public override bool IsClass => false;

    public static implicit operator Interface(IInterfaceIndex index) => index.Interface;

    public override bool Equals(object other) => this.Id.Equals((other as IMetaIdentifiableObject)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return this.SingularName;
        }

        return this.Tag;
    }

    public override RoleType KeyRoleType => this.derivedKeyRoleType;

    public RoleType DerivedKeyRoleType
    {
        get => this.derivedKeyRoleType;
        set => this.derivedKeyRoleType = value;
    }

    public override void Validate(ValidationLog validationLog)
    {
        this.ValidateObjectType(validationLog);
        this.ValidateComposite(validationLog);
    }


    public override IReadOnlyList<Composite> DirectSubtypes => this.directSubtypes;

    public override IReadOnlyList<Composite> Composites => this.composites;

    public override IReadOnlyList<Class> Classes => this.subclasses;

    public override IReadOnlyList<Composite> Subtypes => this.subtypes;

    public override Class ExclusiveClass => this.exclusiveClass;

    public override IEnumerable<string> WorkspaceNames => this.derivedWorkspaceNames;

    public override bool IsAssignableFrom(Composite objectType) =>
        this.Equals(objectType) || this.subtypes.Contains(objectType);

    public void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = ((Interface)this)
            .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
            .Union(((Interface)this).AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
            .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
            .ToArray();

    public void InitializeDirectSubtypes()
    {
        this.directSubtypes = this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)).ToArray();
    }

    public void InitializeSubtypes()
    {
        var subtypes = new HashSet<Composite>();
        this.InitializeSubtypesRecursively(this, subtypes);
        this.subtypes = subtypes.ToArray();
    }

    public void InitializeSubclasses()
    {
        var subclasses = new HashSet<Class>();
        foreach (var subType in this.subtypes.OfType<Class>())
        {
            subclasses.Add((Class)subType);
        }

        this.subclasses = subclasses.ToArray();
    }

    public void InitializeComposites()
    {
        this.composites = this.subtypes.Append(this).ToArray();
    }

    public void InitializeExclusiveSubclass() => this.exclusiveClass = this.subclasses.Count == 1 ? this.subclasses.First() : null;

    private void InitializeSubtypesRecursively(IObjectType type, ISet<Composite> subtypes)
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

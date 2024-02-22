// <copyright file="Class.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Allors.Embedded.Meta;

public sealed class Class : Composite
{
    private ConcurrentDictionary<MethodType, Action<object, object>[]> actionsByMethodType;

    public Class(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.Composites = new[] { this };
        this.Classes = new[] { this };
        this.DirectSubtypes = Array.Empty<Composite>();
        this.Subtypes = Array.Empty<Composite>();

        this.MetaPopulation.OnCreated(this);
    }

    private RoleType derivedKeyRoleType;

    public override bool IsInterface => false;

    public override bool IsClass => true;

    public static implicit operator Class(IClassIndex index) => index.Class;

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

    public string[] AssignedWorkspaceNames { get; set; } = Array.Empty<string>();

    public override IReadOnlyList<Composite> Composites { get; }

    public override IReadOnlyList<Class> Classes { get; }

    public override Class ExclusiveClass => this;

    public override IReadOnlyList<Composite> DirectSubtypes { get; }

    public override IReadOnlyList<Composite> Subtypes { get; }

    public override IEnumerable<string> WorkspaceNames => this.AssignedWorkspaceNames;

    public override bool IsAssignableFrom(Composite objectType) => this.Equals(objectType);
}

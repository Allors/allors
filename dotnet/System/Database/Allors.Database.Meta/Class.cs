// <copyright file="Class.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public abstract class Class : Composite, IClass
{
    private readonly IReadOnlySet<IClass> classes;

    private ConcurrentDictionary<IMethodType, Action<object, object>[]> actionsByMethodType;
    private IRoleType[] derivedRequiredRoleTypes;

    protected Class(MetaPopulation metaPopulation, Guid id, Interface[] directSupertypes, string singularName, string assignedPluralName)
        : base(metaPopulation, id, directSupertypes, singularName, assignedPluralName)
    {
        // TODO: Create single element IReadOnlySet
        this.classes = new HashSet<IClass> { this };
        this.Subtypes = MetaPopulation.EmptyComposites;
        metaPopulation.OnCreated(this);
    }

    public string[] AssignedWorkspaceNames { get; set; } = Array.Empty<string>();

    public override IReadOnlySet<IClass> Classes => this.classes;

    public override IClass ExclusiveClass => this;

    public override IReadOnlySet<IComposite> Subtypes { get; }

    public IRoleType[] OverriddenRequiredRoleTypes { get; set; } = Array.Empty<IRoleType>();

    public IRoleType[] RequiredRoleTypes
    {
        get
        {
            return this.derivedRequiredRoleTypes;
        }
    }

    public override IEnumerable<string> WorkspaceNames => this.AssignedWorkspaceNames;

    public override bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);

    public Action<object, object>[] Actions(IMethodType methodType)
    {
        this.actionsByMethodType ??= new ConcurrentDictionary<IMethodType, Action<object, object>[]>();
        if (!this.actionsByMethodType.TryGetValue(methodType, out var actions))
        {
            actions = this.MetaPopulation.MethodCompiler.Compile(this, methodType);
            this.actionsByMethodType[methodType] = actions;
        }

        return actions;
    }

    internal void DeriveRequiredRoleTypes() =>
        this.derivedRequiredRoleTypes = this.RoleTypes
            .Where(v => v.IsRequired)
            .Union(this.OverriddenRequiredRoleTypes).ToArray();
}

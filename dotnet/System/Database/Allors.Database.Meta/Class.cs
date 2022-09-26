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
    private readonly Class[] classes;

    private ConcurrentDictionary<IMethodType, Action<object, object>[]> actionsByMethodType;
    private IRoleType[] derivedRequiredRoleTypes;
    private string[] derivedWorkspaceNames;

    protected Class(MetaPopulation metaPopulation, Guid id, string singularName, string assignedPluralName)
        : base(metaPopulation, id, singularName, assignedPluralName)
    {
        this.classes = new[] { this };
        metaPopulation.OnCreated(this);
    }

    public string[] AssignedWorkspaceNames { get; set; }

    public override IEnumerable<Class> Classes => this.classes;

    public override Class ExclusiveClass => this;

    public override IEnumerable<Composite> Subtypes => Array.Empty<Composite>();

    public IRoleType[] OverriddenRequiredRoleTypes { get; set; } = Array.Empty<IRoleType>();

    public IRoleType[] RequiredRoleTypes
    {
        get
        {
            return this.derivedRequiredRoleTypes;
        }
    }

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            return this.derivedWorkspaceNames;
        }
    }

    public override bool ExistClass => true;

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

    internal void DeriveWorkspaceNames(HashSet<string> workspaceNames)
    {
        this.derivedWorkspaceNames = this.AssignedWorkspaceNames ?? Array.Empty<string>();
        workspaceNames.UnionWith(this.derivedWorkspaceNames);
    }

    internal void DeriveRequiredRoleTypes() =>
        this.derivedRequiredRoleTypes = this.RoleTypes
            .Where(v => v.IsRequired)
            .Union(this.OverriddenRequiredRoleTypes).ToArray();
}

﻿// <copyright file="Class.cs" company="Allors bvba">
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
    private string[] assignedWorkspaceNames;
    private IRoleType[] derivedRequiredRoleTypes;
    private string[] derivedWorkspaceNames;

    private IRoleType[] overriddenRequiredRoleTypes;

    protected Class(MetaPopulation metaPopulation, Guid id, string tag)
        : base(metaPopulation, id, tag)
    {
        this.classes = new[] { this };
        metaPopulation.OnClassCreated(this);
    }

    public string[] AssignedWorkspaceNames
    {
        get => this.assignedWorkspaceNames;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.assignedWorkspaceNames = value;
            this.MetaPopulation.Stale();
        }
    }

    public override IEnumerable<Class> Classes => this.classes;

    public override Class ExclusiveClass => this;

    public override IEnumerable<Composite> Subtypes => Array.Empty<Composite>();

    public long CreatePermissionId { get; set; }

    public IReadOnlyDictionary<Guid, long> ReadPermissionIdByRelationTypeId { get; set; }

    public IReadOnlyDictionary<Guid, long> WritePermissionIdByRelationTypeId { get; set; }

    public IReadOnlyDictionary<Guid, long> ExecutePermissionIdByMethodTypeId { get; set; }

    public IRoleType[] OverriddenRequiredRoleTypes
    {
        get => this.overriddenRequiredRoleTypes ?? Array.Empty<IRoleType>();

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.overriddenRequiredRoleTypes = value;
            this.MetaPopulation.Stale();
        }
    }

    public IRoleType[] RequiredRoleTypes
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedRequiredRoleTypes;
        }
    }

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    public override bool ExistClass => true;

    public override bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);

    public override void Bind(Dictionary<string, Type> typeByTypeName) => this.ClrType = typeByTypeName[this.Name];

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
        this.derivedWorkspaceNames = this.assignedWorkspaceNames ?? Array.Empty<string>();
        workspaceNames.UnionWith(this.derivedWorkspaceNames);
    }

    internal void DeriveRequiredRoleTypes() =>
        this.derivedRequiredRoleTypes = this.RoleTypes
            .Where(v => v.IsRequired)
            .Union(this.OverriddenRequiredRoleTypes).ToArray();
}

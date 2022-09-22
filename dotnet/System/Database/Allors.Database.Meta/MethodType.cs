// <copyright file="MethodInterface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class MethodType : MetaIdentifiableObject, IMethodType, IComparable
{
    private string[] assignedWorkspaceNames;
    private string[] derivedWorkspaceNames;

    private Record input;

    private string name;
    private Record output;

    public MethodType(Composite objectType, Guid id, string name, Record input, Record output)
    : base(objectType.MetaPopulation, id)
    {
        this.ObjectType = objectType;
        this.Name = name;
        this.Input = input;
        this.Output = output;

        this.MetaPopulation.OnMethodTypeCreated(this);
    }

    IComposite IMethodType.ObjectType => this.ObjectType;

    public Composite ObjectType { get; }

    public string[] AssignedWorkspaceNames
    {
        get => this.assignedWorkspaceNames ?? Array.Empty<string>();

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.assignedWorkspaceNames = value;
            this.MetaPopulation.Stale();
        }
    }

    public string Name { get; }

    public string FullName => $"{this.ObjectType.Name}{this.Name}";


    IRecord IMethodType.Input => this.Input;

    public Record Input { get; }

    IRecord IMethodType.Output => this.Output;

    public Record Output { get; }

    public string DisplayName => this.Name;

    private string ValidationName
    {
        get
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                return "method type " + this.Name;
            }

            return "unknown method type";
        }
    }

    public int CompareTo(object other) => this.Id.CompareTo((other as MethodType)?.Id);

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    public override bool Equals(object other) => this.Id.Equals((other as MethodType)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString() => this.Name;

    public void Validate(ValidationLog validationLog)
    {
        if (string.IsNullOrEmpty(this.Name))
        {
            var message = this.ValidationName + " has no name";
            validationLog.AddError(message, this, ValidationKind.Required, "MethodType.Name");
        }
    }

    internal void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this.assignedWorkspaceNames != null
            ? this.assignedWorkspaceNames
                .Intersect(this.ObjectType.Classes.SelectMany(v => v.WorkspaceNames))
                .ToArray()
            : Array.Empty<string>();

    internal void PrepareWorkspaceNames(IDictionary<Record, ISet<string>> workspaceNamesByRecord)
    {
        var visited = new HashSet<Record>();

        if (this.derivedWorkspaceNames.Length > 0)
        {
            this.Input?.PrepareWorkspaceNames(workspaceNamesByRecord, visited, this.derivedWorkspaceNames);
            this.Output?.PrepareWorkspaceNames(workspaceNamesByRecord, visited, this.derivedWorkspaceNames);
        }
    }
}

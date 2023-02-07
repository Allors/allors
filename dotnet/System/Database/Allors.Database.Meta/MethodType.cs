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
    private string[] derivedWorkspaceNames;

    public MethodType(Composite objectType, Guid id, string name)
    : base(objectType.MetaPopulation, id)
    {
        this.ObjectType = objectType;
        this.Name = name;

        this.CompositeMethodType = new CompositeMethodType(objectType, this);

        this.MetaPopulation.OnCreated(this);
    }

    public ICompositeMethodType CompositeMethodType { get; }

    public IReadOnlyDictionary<IComposite, ICompositeMethodType> CompositeMethodTypeByComposite { get; private set; }

    IComposite IMethodType.ObjectType => this.ObjectType;

    public Composite ObjectType { get; }

    public IReadOnlyList<string> AssignedWorkspaceNames { get; set; }

    public string Name { get; }

    IRecordType IMethodType.Input => this.Input;

    public Record Input { get; }

    IRecordType IMethodType.Output => this.Output;

    public Record Output { get; }

    public string DisplayName => this.Name;

    public override IEnumerable<string> WorkspaceNames
    {
        get
        {
            return this.derivedWorkspaceNames;
        }
    }

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
        this.derivedWorkspaceNames = this.AssignedWorkspaceNames != null
            ? this.AssignedWorkspaceNames
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

    internal void InitializeCompositeMethodTypes(Dictionary<IComposite, HashSet<ICompositeMethodType>> compositeMethodTypesByComposite)
    {
        var composite = this.ObjectType;
        compositeMethodTypesByComposite[composite].Add(this.CompositeMethodType);

        var dictionary = composite.Subtypes.ToDictionary(v => v, v =>
        {
            var compositeMethodType = (ICompositeMethodType)new CompositeMethodType(v, this);
            compositeMethodTypesByComposite[v].Add(compositeMethodType);
            return compositeMethodType;
        });

        dictionary[composite] = this.CompositeMethodType;

        this.CompositeMethodTypeByComposite = dictionary;
    }
}

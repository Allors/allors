// <copyright file="MethodInterface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class MethodType : IStaticMethodType, IComparable, IMetaIdentifiableObject
{
    private string[] derivedWorkspaceNames;

    public MethodType(IComposite objectType, Guid id, string name)
    {
        this.Attributes = new MetaExtension();
        this.MetaPopulation = (MetaPopulation)objectType.MetaPopulation;
        this.Id = id;
        this.Tag = id.Tag();
        this.ObjectType = objectType;
        this.Name = name;
        this.AssignedWorkspaceNames = Array.Empty<string>();

        this.CompositeMethodType = new CompositeMethodType(objectType, this);

        this.MetaPopulation.OnCreated(this);
    }

    public dynamic Attributes { get; }

    IMetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public IStaticMetaPopulation MetaPopulation { get; }

    public Guid Id { get; }

    public string Tag { get; set; }
    
    public ICompositeMethodType CompositeMethodType { get; }

    public IReadOnlyDictionary<IComposite, ICompositeMethodType> CompositeMethodTypeByComposite { get; private set; }

    IComposite IMethodType.ObjectType => this.ObjectType;

    public IComposite ObjectType { get; }

    public IReadOnlyList<string> AssignedWorkspaceNames { get; set; }

    public string Name { get; }

    public IEnumerable<string> WorkspaceNames
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

    void IStaticMethodType.DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this.AssignedWorkspaceNames != null
            ? this.AssignedWorkspaceNames
                .Intersect(this.ObjectType.Classes.SelectMany(v => v.WorkspaceNames))
                .ToArray()
            : Array.Empty<string>();

    void IStaticMethodType.InitializeCompositeMethodTypes(Dictionary<IComposite, HashSet<ICompositeMethodType>> compositeMethodTypesByComposite)
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

// <copyright file="MethodInterface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Embedded.Meta;

public sealed class MethodType : OperandType, IComparable 
{
    private string[] derivedWorkspaceNames;

    public MethodType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
     
        this.AssignedWorkspaceNames = Array.Empty<string>();

        //this.CompositeMethodType = new CompositeMethodType(objectType, this);

        this.MetaPopulation.OnCreated(this);
    }

  

    public CompositeMethodType CompositeMethodType { get; set; }

    public IReadOnlyDictionary<Composite, CompositeMethodType> CompositeMethodTypeByComposite { get; private set; }

    public Composite ObjectType { get; set; }

    public IReadOnlyList<string> AssignedWorkspaceNames { get; set; }

    public string Name { get; set; }

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

    public override string ToString() => this.Name;

    public override void Validate(ValidationLog validationLog)
    {
        if (string.IsNullOrEmpty(this.Name))
        {
            var message = this.ValidationName + " has no name";
            validationLog.AddError(message, this, ValidationKind.Required, "MethodType.Name");
        }
    }

    public void DeriveWorkspaceNames() =>
        this.derivedWorkspaceNames = this.AssignedWorkspaceNames != null
            ? this.AssignedWorkspaceNames
                .Intersect(this.ObjectType.Classes.SelectMany(v => v.WorkspaceNames))
                .ToArray()
            : Array.Empty<string>();

    public void InitializeCompositeMethodTypes(Dictionary<Composite, HashSet<CompositeMethodType>> compositeMethodTypesByComposite)
    {
        var composite = this.ObjectType;
        compositeMethodTypesByComposite[composite].Add(this.CompositeMethodType);

        var dictionary = composite.Subtypes.ToDictionary(v => v, v =>
        {
            var compositeMethodType = new CompositeMethodType(v, this);
            compositeMethodTypesByComposite[v].Add(compositeMethodType);
            return compositeMethodType;
        });

        dictionary[composite] = this.CompositeMethodType;

        this.CompositeMethodTypeByComposite = dictionary;
    }
}

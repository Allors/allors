// <copyright file="ObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public abstract class DataType : IDataType
{
    protected DataType(MetaPopulation metaPopulation, Guid id, string tag = null)
    {
        this.MetaPopulation = metaPopulation;
        this.Id = id;
        this.Tag = tag ?? id.Tag();
    }

    IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

    public MetaPopulation MetaPopulation { get; }

    public Guid Id { get; }

    public string Tag { get; }

    public abstract string Name { get; }

    public Type ClrType { get; set; }

    public abstract IEnumerable<string> WorkspaceNames { get; }
}

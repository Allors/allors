// <copyright file="Extent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System.Collections;
using System.Collections.Generic;
using Meta;

internal abstract class Extent : Allors.Database.Extent
{
    internal abstract SqlExtent InExtent { get; }
    IEnumerator<IObject> IEnumerable<IObject>.GetEnumerator()
    {
        foreach (var @object in this)
        {
            yield return (IObject)@object;
        }
    }

    public abstract int Count { get; }

    public abstract ICompositePredicate Filter { get; }

    public abstract Composite ObjectType { get; }

    public abstract Allors.Database.Extent AddSort(RoleType roleType);

    public abstract Allors.Database.Extent AddSort(RoleType roleType, SortDirection direction);

    public abstract IEnumerator GetEnumerator();
}

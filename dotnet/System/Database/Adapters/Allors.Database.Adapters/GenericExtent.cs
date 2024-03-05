// <copyright file="ArrayExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta;

public class GenericExtent<T>(IExtent<IObject> extent) : IExtent<T>
    where T : class, IObject
{
    public IEnumerator<T> GetEnumerator() => extent.Cast<T>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => extent.GetEnumerator();

    public int Count => extent.Count;

    public ICompositePredicate Filter => extent.Filter;

    public Composite ObjectType => extent.ObjectType;

    public IExtent<IObject> AddSort(RoleType roleType)
    {
        return extent.AddSort(roleType);
    }

    public IExtent<IObject> AddSort(RoleType roleType, SortDirection direction)
    {
        return extent.AddSort(roleType, direction);
    }
}

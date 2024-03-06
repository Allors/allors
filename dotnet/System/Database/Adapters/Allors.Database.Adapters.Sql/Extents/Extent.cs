// <copyright file="SqlExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System.Collections;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class Extent : IInternalExtent, IExtent<IObject>
{
    private IList<long> objectIds;
    
    IEnumerator<IObject> IEnumerable<IObject>.GetEnumerator()
    {
        foreach (var @object in this)
        {
            yield return (IObject)@object;
        }
    }

    public abstract ICompositePredicate Filter { get; }

    public abstract Composite ObjectType { get; }

    public IExtent<TResult> Cast<TResult>() where TResult : class, IObject
    {
        return new GenericExtent<TResult>(this);
    }

    public int Count => this.ObjectIds.Count;

    public IInternalExtent InExtent => this;

    public IInternalExtent ParentOperationExtent { get; set; }

    public abstract Transaction Transaction { get; }

    public ExtentSort Sorter { get; private set; }

    private IList<long> ObjectIds => this.objectIds ??= this.GetObjectIds();
    
    public Allors.Database.IExtent<IObject> AddSort(RoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

    public Allors.Database.IExtent<IObject> AddSort(RoleType roleType, SortDirection direction)
    {
        this.LazyLoadFilter();
        this.FlushCache();
        if (this.Sorter == null)
        {
            this.Sorter = new ExtentSort(this.Transaction, roleType, direction);
        }
        else
        {
            this.Sorter.AddSort(roleType, direction);
        }

        return this;
    }

    public IEnumerator GetEnumerator()
    {
        var references = this.Transaction.GetOrCreateReferencesForExistingObjects(this.ObjectIds);
        return new ExtentEnumerator(references);
    }

    public abstract string BuildSql(ExtentStatement statement);

    public void FlushCache()
    {
        this.objectIds = null;
        this.ParentOperationExtent?.FlushCache();
    }

    protected abstract IList<long> GetObjectIds();

    protected abstract void LazyLoadFilter();
}

// <copyright file="SqlExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections;
using System.Collections.Generic;
using Allors.Database.Meta;

internal abstract class SqlExtent : Extent
{
    private IList<long> objectIds;

    public override int Count => this.ObjectIds.Count;

    internal override SqlExtent InExtent => this;

    internal ExtentOperation ParentOperationExtent { get; set; }

    internal abstract Transaction Transaction { get; }

    internal ExtentSort Sorter { get; private set; }

    private IList<long> ObjectIds => this.objectIds ??= this.GetObjectIds();
    
    public override Allors.Database.Extent AddSort(RoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

    public override Allors.Database.Extent AddSort(RoleType roleType, SortDirection direction)
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

    public override IEnumerator GetEnumerator()
    {
        var references = this.Transaction.GetOrCreateReferencesForExistingObjects(this.ObjectIds);
        return new ExtentEnumerator(references);
    }

    internal abstract string BuildSql(ExtentStatement statement);

    internal void FlushCache()
    {
        this.objectIds = null;
        this.ParentOperationExtent?.FlushCache();
    }

    protected abstract IList<long> GetObjectIds();

    protected abstract void LazyLoadFilter();
}

// <copyright file="Extent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsExtentMemory type.
// </summary>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class Extent : IExtent<IObject>
{
    private IObject[] defaultObjectArray;
    private Extent parent;

    protected Extent(Transaction transaction) => this.Transaction = transaction;

    IEnumerator<IObject> IEnumerable<IObject>.GetEnumerator()
    {
        foreach (var @object in this)
        {
            yield return (IObject)@object;
        }
    }

    public abstract Composite ObjectType { get; }

    public abstract ICompositePredicate Filter { get; }

    public int Count
    {
        get
        {
            this.Evaluate();
            return this.Strategies.Count;
        }
    }

    public Extent Parent
    {
        get => this.parent;

        set
        {
            if (this.parent != null)
            {
                throw new ArgumentException("Extent has already a parent");
            }

            this.parent = value;
        }
    }

    internal Transaction Transaction { get; }

    internal ExtentSort Sorter { get; private set; }

    protected List<Strategy> Strategies { get; set; }

    public Allors.Database.IExtent<IObject> AddSort(RoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

    public Allors.Database.IExtent<IObject> AddSort(RoleType roleType, SortDirection direction)
    {
        if (this.Sorter == null)
        {
            this.Sorter = new ExtentSort(roleType, direction);
        }
        else
        {
            this.Sorter.AddSort(roleType, direction);
        }

        this.Invalidate();
        return this;
    }
    
    public IEnumerator GetEnumerator()
    {
        this.Evaluate();
        return new ExtentEnumerator(this.Strategies.GetEnumerator());
    }

    internal List<Strategy> GetEvaluatedStrategies()
    {
        this.Evaluate();
        return this.Strategies;
    }

    internal void Invalidate()
    {
        this.Strategies = null;
        this.parent?.Invalidate();
    }

    protected abstract void Evaluate();
}

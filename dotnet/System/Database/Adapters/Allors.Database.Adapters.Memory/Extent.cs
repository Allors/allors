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

public abstract class Extent<T> : IExtentOperand, IInternalExtent, IExtent<T> where T : class, IObject
{
    private IInternalExtent parent;

    protected Extent(Transaction transaction) => this.Transaction = transaction;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        foreach (var @object in this)
        {
            yield return (T)@object;
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

    public IInternalExtent Parent
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

    public IExtent<IObject> AddSort(RoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

    public IExtent<IObject> AddSort(RoleType roleType, SortDirection direction)
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

    public IExtent<TResult> Cast<TResult>() where TResult : class, IObject
    {
        return (IExtent<TResult>)this;
    }

    public IEnumerator GetEnumerator()
    {
        this.Evaluate();
        return new ExtentEnumerator(this.Strategies.GetEnumerator());
    }

    public IEnumerable<Strategy> GetEvaluatedStrategies()
    {
        this.Evaluate();
        return this.Strategies;
    }

    public void CheckForAssociationType(AssociationType association)
    {
        if (!this.Transaction.Database.MetaCache.GetAssociationTypesByComposite(this.ObjectType).Contains(association))
        {
            throw new ArgumentException("Extent does not have association " + association);
        }
    }

    public void CheckForRoleType(RoleType roleType)
    {
        if (!this.Transaction.Database.MetaCache.GetRoleTypesByComposite(this.ObjectType).Contains(roleType))
        {
            throw new ArgumentException("Extent does not have role " + roleType.SingularName);
        }
    }

    public void Invalidate()
    {
        this.Strategies = null;
        this.parent?.Invalidate();
    }

    protected abstract void Evaluate();
}

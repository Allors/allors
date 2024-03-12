// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class ExtentFilter<T> : Extent<T>, IFilter<T> where T : class, IObject
{
    private And filter;

    public ExtentFilter(Transaction transaction, Composite objectType)
        : base(transaction) =>
        this.ObjectType = objectType;

    public override ICompositePredicate Predicate
    {
        get
        {
            this.filter ??= new And(this);
            return this.filter;
        }
    }

    public override Composite ObjectType { get; }

    protected override void Evaluate()
    {
        if (this.Strategies == null)
        {
            this.Strategies = new List<Strategy>();

            foreach (var strategy in this.Transaction.GetStrategiesForExtentIncludingDeleted(this.ObjectType))
            {
                if (!strategy.IsDeleted)
                {
                    if (this.filter?.Include != true || this.filter.Evaluate(strategy) == ThreeValuedLogic.True)
                    {
                        this.Strategies.Add(strategy);
                    }
                }
            }

            if (this.Sorter != null)
            {
                this.Strategies.Sort(this.Sorter);
            }
        }
    }

    ICompositePredicate ICompositePredicate.AddAnd(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddAnd(init);
    }

    IPredicate ICompositePredicate.AddBetween(RoleType role, object firstValue, object secondValue)
    {
        return this.Predicate.AddBetween(role, firstValue, secondValue);
    }

    IPredicate ICompositePredicate.AddIntersects(RoleType role, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddIntersects(role, containingExtent);
    }

    IPredicate ICompositePredicate.AddIntersects(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddIntersects(role, containingEnumerable);
    }

    IPredicate ICompositePredicate.AddIntersects(AssociationType association, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddIntersects(association, containingExtent);
    }

    IPredicate ICompositePredicate.AddIntersects(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddIntersects(association, containingEnumerable);
    }
    
    IPredicate ICompositePredicate.AddIn(RoleType role, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddIn(role, containingExtent);
    }

    IPredicate ICompositePredicate.AddIn(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddIn(role, containingEnumerable);
    }

    IPredicate ICompositePredicate.AddIn(AssociationType association, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddIn(association, containingExtent);
    }

    IPredicate ICompositePredicate.AddIn(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddIn(association, containingEnumerable);
    }

    IPredicate ICompositePredicate.AddHas(RoleType role, IObject containedObject)
    {
        return this.Predicate.AddHas(role, containedObject);
    }

    IPredicate ICompositePredicate.AddHas(AssociationType association, IObject containedObject)
    {
        return this.Predicate.AddHas(association, containedObject);
    }

    IPredicate ICompositePredicate.AddEquals(IObject allorsObject)
    {
        return this.Predicate.AddEquals(allorsObject);
    }

    IPredicate ICompositePredicate.AddEquals(RoleType roleType, object valueOrAllorsObject)
    {
        return this.Predicate.AddEquals(roleType, valueOrAllorsObject);
    }

    IPredicate ICompositePredicate.AddEquals(AssociationType association, IObject allorsObject)
    {
        return this.Predicate.AddEquals(association, allorsObject);
    }

    IPredicate ICompositePredicate.AddExists(RoleType role)
    {
        return this.Predicate.AddExists(role);
    }

    IPredicate ICompositePredicate.AddExists(AssociationType association)
    {
        return this.Predicate.AddExists(association);
    }

    IPredicate ICompositePredicate.AddGreaterThan(RoleType role, object value)
    {
        return this.Predicate.AddGreaterThan(role, value);
    }

    IPredicate ICompositePredicate.AddInstanceOf(Composite objectType)
    {
        return this.Predicate.AddInstanceOf(objectType);
    }

    IPredicate ICompositePredicate.AddInstanceOf(RoleType role, Composite objectType)
    {
        return this.Predicate.AddInstanceOf(role, objectType);
    }

    IPredicate ICompositePredicate.AddInstanceOf(AssociationType association, Composite objectType)
    {
        return this.Predicate.AddInstanceOf(association, objectType);
    }

    IPredicate ICompositePredicate.AddLessThan(RoleType role, object value)
    {
        return this.Predicate.AddLessThan(role, value);
    }

    IPredicate ICompositePredicate.AddLike(RoleType role, string value)
    {
        return this.Predicate.AddLike(role, value);
    }

    ICompositePredicate ICompositePredicate.AddNot(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddNot(init);
    }

    ICompositePredicate ICompositePredicate.AddOr(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddOr(init);
    }
}

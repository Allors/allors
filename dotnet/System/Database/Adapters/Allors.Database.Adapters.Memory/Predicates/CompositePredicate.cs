// <copyright file="CompositePredicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal abstract class CompositePredicate : Predicate, ICompositePredicate
{
    private readonly IInternalExtent extent;

    internal CompositePredicate(IInternalExtent extent)
    {
        this.extent = extent;
        this.Filters = new List<Predicate>(4);
    }

    internal override bool Include
    {
        get
        {
            foreach (var filter in this.Filters)
            {
                if (filter.Include)
                {
                    return true;
                }
            }

            return false;
        }
    }

    protected internal List<Predicate> Filters { get; }

    public ICompositePredicate AddAnd(Action<ICompositePredicate> init = null)
    {
        var and = new And(this.extent);
        init?.Invoke(and);

        this.extent.Invalidate();
        this.Filters.Add(and);
        return and;
    }

    public IPredicate AddBetween(RoleType role, object firstValue, object secondValue)
    {
        var between = new Between(this.extent, role, firstValue, secondValue);

        this.extent.Invalidate();
        this.Filters.Add(between);
        return between;
    }

    public IPredicate AddWithin(RoleType role, Allors.Database.IExtent<IObject> containingExtent)
    {
        Within within = role.IsMany
            ? new IntersectsRoleExtent(this.extent, role, containingExtent)
            : new WithinRoleExtent(this.extent, role, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(within);
        return within;
    }

    public IPredicate AddIntersects(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        Within within = role.IsMany
            ? new IntersectsRoleEnumerable(this.extent, role, containingEnumerable)
            : new WithinRoleEnumerable(this.extent, role, containingEnumerable);

        this.extent.Invalidate();
        this.Filters.Add(within);
        return within;
    }

    public IPredicate AddIntersects(AssociationType association, Allors.Database.IExtent<IObject> containingExtent)
    {
        Within containedIn = association.IsMany
            ? new IntersectsAssociationExtent(this.extent, association, containingExtent)
            : new WithinAssociationExtent(this.extent, association, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddIntersects(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        Within containedIn = association.IsMany
            ? new IntersectsAssociationEnumerable(this.extent, association, containingEnumerable)
            : new WithinAssociationEnumerable(this.extent, association, containingEnumerable);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }
    
    public IPredicate AddIntersects(RoleType role, Allors.Database.IExtent<IObject> containingExtent)
    {
        Within within = role.IsMany
            ? new IntersectsRoleExtent(this.extent, role, containingExtent)
            : new WithinRoleExtent(this.extent, role, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(within);
        return within;
    }

    public IPredicate AddWithin(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        Within within = role.IsMany
            ? new IntersectsRoleEnumerable(this.extent, role, containingEnumerable)
            : new WithinRoleEnumerable(this.extent, role, containingEnumerable);

        this.extent.Invalidate();
        this.Filters.Add(within);
        return within;
    }

    public IPredicate AddWithin(AssociationType association, Allors.Database.IExtent<IObject> containingExtent)
    {
        Within containedIn = association.IsMany
            ? new IntersectsAssociationExtent(this.extent, association, containingExtent)
            : new WithinAssociationExtent(this.extent, association, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddWithin(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        Within containedIn = association.IsMany
        ? new IntersectsAssociationEnumerable(this.extent, association, containingEnumerable)
        : new WithinAssociationEnumerable(this.extent, association, containingEnumerable);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddContains(RoleType role, IObject containedObject)
    {
        var contains = new ContainsRole(this.extent, role, containedObject);

        this.extent.Invalidate();
        this.Filters.Add(contains);
        return contains;
    }

    public IPredicate AddContains(AssociationType association, IObject containedObject)
    {
        var contains = new ContainsAssociation(this.extent, association, containedObject);

        this.extent.Invalidate();
        this.Filters.Add(contains);
        return contains;
    }

    public IPredicate AddEquals(IObject allorsObject)
    {
        var equals = new EqualsComposite(allorsObject);

        this.extent.Invalidate();
        this.Filters.Add(equals);
        return equals;
    }

    public IPredicate AddEquals(RoleType role, object obj)
    {
        Equals equals = role.ObjectType is Unit
            ? new EqualsRoleUnit(this.extent, role, obj)
            : new EqualsRoleComposite(this.extent, role, obj);

        this.extent.Invalidate();
        this.Filters.Add(equals);
        return equals;
    }

    public IPredicate AddEquals(AssociationType association, IObject allorsObject)
    {
        var equals = new EqualsAssociation(this.extent, association, allorsObject);

        this.extent.Invalidate();
        this.Filters.Add(equals);
        return equals;
    }

    public IPredicate AddExists(RoleType role)
    {
        var exists = new ExistsRole(this.extent, role);

        this.extent.Invalidate();
        this.Filters.Add(exists);
        return exists;
    }

    public IPredicate AddExists(AssociationType association)
    {
        var exists = new ExistsAssociation(this.extent, association);

        this.extent.Invalidate();
        this.Filters.Add(exists);
        return exists;
    }

    public IPredicate AddGreaterThan(RoleType role, object value)
    {
        var greaterThan = new GreaterThan(this.extent, role, value);

        this.extent.Invalidate();
        this.Filters.Add(greaterThan);
        return greaterThan;
    }

    public IPredicate AddInstanceOf(Composite type)
    {
        var instanceOf = new InstanceOfObject(type);

        this.extent.Invalidate();
        this.Filters.Add(instanceOf);
        return instanceOf;
    }

    public IPredicate AddInstanceOf(RoleType role, Composite type)
    {
        var instanceOf = new InstanceOfRole(this.extent, role, type);

        this.extent.Invalidate();
        this.Filters.Add(instanceOf);
        return instanceOf;
    }

    public IPredicate AddInstanceOf(AssociationType association, Composite type)
    {
        var instanceOf = new InstanceOfAssociation(this.extent, association, type);

        this.extent.Invalidate();
        this.Filters.Add(instanceOf);
        return instanceOf;
    }

    public IPredicate AddLessThan(RoleType role, object value)
    {
        var lessThan = new LessThan(this.extent, role, value);

        this.extent.Invalidate();
        this.Filters.Add(lessThan);
        return lessThan;
    }

    public IPredicate AddLike(RoleType role, string value)
    {
        var like = new Like(this.extent, role, value);

        this.extent.Invalidate();
        this.Filters.Add(like);
        return like;
    }

    public ICompositePredicate AddNot(Action<ICompositePredicate> init = null)
    {
        var not = new Not(this.extent);
        init?.Invoke(not);

        this.extent.Invalidate();
        this.Filters.Add(not);
        return not;
    }

    public ICompositePredicate AddOr(Action<ICompositePredicate> init = null)
    {
        var or = new Or(this.extent);
        init?.Invoke(or);

        this.extent.Invalidate();
        this.Filters.Add(or);
        return or;
    }
}

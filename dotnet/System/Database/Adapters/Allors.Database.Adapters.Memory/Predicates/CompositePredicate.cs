// <copyright file="CompositePredicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Collections.Generic;
using Allors.Database.Meta;

internal abstract class CompositePredicate : Predicate, ICompositePredicate
{
    private readonly ExtentFiltered extent;

    internal CompositePredicate(ExtentFiltered extent)
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

    public ICompositePredicate AddAnd()
    {
        var andFilter = new And(this.extent);

        this.extent.Invalidate();
        this.Filters.Add(andFilter);
        return andFilter;
    }

    public IPredicate AddBetween(RoleType role, object firstValue, object secondValue)
    {
        var between = new Between(this.extent, role, firstValue, secondValue);

        this.extent.Invalidate();
        this.Filters.Add(between);
        return between;
    }

    public IPredicate AddContainedIn(RoleType role, Allors.Database.Extent containingExtent)
    {
        ContainedIn containedIn = role.IsMany
            ? new ContainedInRoleManyExtent(this.extent, role, containingExtent)
            : new ContainedInRoleOneExtent(this.extent, role, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddContainedIn(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        ContainedIn containedIn = role.IsMany
            ? new ContainedInRoleManyEnumerable(this.extent, role, containingEnumerable)
            : new ContainedInRoleOneEnumerable(this.extent, role, containingEnumerable);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddContainedIn(AssociationType association, Allors.Database.Extent containingExtent)
    {
        var containedIn = new ContainedInAssociationExtent(this.extent, association, containingExtent);

        this.extent.Invalidate();
        this.Filters.Add(containedIn);
        return containedIn;
    }

    public IPredicate AddContainedIn(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        var containedIn = new ContainedInAssociationEnumerable(this.extent, association, containingEnumerable);

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

    public ICompositePredicate AddNot()
    {
        var not = new Not(this.extent);

        this.extent.Invalidate();
        this.Filters.Add(not);
        return not;
    }

    public ICompositePredicate AddOr()
    {
        var or = new Or(this.extent);

        this.extent.Invalidate();
        this.Filters.Add(or);
        return or;
    }
}

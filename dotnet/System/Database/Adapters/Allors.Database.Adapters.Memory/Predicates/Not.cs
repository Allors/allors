// <copyright file="Not.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class Not : Predicate, ICompositePredicate
{
    private readonly ExtentFiltered extent;
    private Predicate predicate;

    internal Not(ExtentFiltered extent) => this.extent = extent;

    internal override bool Include => this.predicate?.Include == true;

    public ICompositePredicate AddAnd(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();

        var and = new And(this.extent);
        init?.Invoke(and);

        this.extent.Invalidate();
        this.predicate = and;
        return (ICompositePredicate)this.predicate;
    }

    public IPredicate AddBetween(RoleType role, object firstValue, object secondValue)
    {
        this.CheckUnarity();

        var between = new Between(this.extent, role, firstValue, secondValue);

        this.extent.Invalidate();
        this.predicate = between;
        return between;
    }

    public IPredicate AddIn(RoleType role, Allors.Database.Extent containingExtent)
    {
        this.CheckUnarity();

        In @in = role.IsMany ?
            new InRoleManyExtent(this.extent, role, containingExtent) :
            new InRoleOneExtent(this.extent, role, containingExtent);

        this.extent.Invalidate();
        this.predicate = @in;
        return @in;
    }

    public IPredicate AddIn(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        this.CheckUnarity();

        In @in = role.IsMany ?
            new InRoleManyEnumerable(this.extent, role, containingEnumerable) :
            new InRoleOneEnumerable(this.extent, role, containingEnumerable);

        this.extent.Invalidate();
        this.predicate = @in;
        return @in;
    }

    public IPredicate AddIn(AssociationType association, Allors.Database.Extent containingExtent)
    {
        this.CheckUnarity();

        var containedIn = new InAssociationExtent(this.extent, association, containingExtent);

        this.extent.Invalidate();
        this.predicate = containedIn;
        return containedIn;
    }

    public IPredicate AddIn(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        this.CheckUnarity();

        var containedIn = new InAssociationEnumerable(this.extent, association, containingEnumerable);

        this.extent.Invalidate();
        this.predicate = containedIn;
        return containedIn;
    }

    public IPredicate AddContains(RoleType role, IObject containedObject)
    {
        this.CheckUnarity();

        var contains = new ContainsRole(this.extent, role, containedObject);

        this.extent.Invalidate();
        this.predicate = contains;
        return contains;
    }

    public IPredicate AddContains(AssociationType association, IObject containedObject)
    {
        this.CheckUnarity();

        var contains = new ContainsAssociation(this.extent, association, containedObject);

        this.extent.Invalidate();
        this.predicate = contains;
        return contains;
    }

    public IPredicate AddEquals(IObject allorsObject)
    {
        this.CheckUnarity();

        var equals = new EqualsComposite(allorsObject);

        this.extent.Invalidate();
        this.predicate = equals;
        return equals;
    }

    public IPredicate AddEquals(RoleType role, object obj)
    {
        this.CheckUnarity();

        Equals equals = role.ObjectType is Unit
            ? new EqualsRoleUnit(this.extent, role, obj)
            : new EqualsRoleComposite(this.extent, role, obj);

        this.extent.Invalidate();
        this.predicate = equals;
        return equals;
    }

    public IPredicate AddEquals(AssociationType association, IObject allorsObject)
    {
        this.CheckUnarity();

        var equals = new EqualsAssociation(this.extent, association, allorsObject);

        this.extent.Invalidate();
        this.predicate = equals;
        return equals;
    }

    public IPredicate AddExists(RoleType role)
    {
        this.CheckUnarity();

        var exists = new ExistsRole(this.extent, role);

        this.extent.Invalidate();
        this.predicate = exists;
        return exists;
    }

    public IPredicate AddExists(AssociationType association)
    {
        this.CheckUnarity();

        var exists = new ExistsAssociation(this.extent, association);

        this.extent.Invalidate();
        this.predicate = exists;
        return exists;
    }

    public IPredicate AddGreaterThan(RoleType role, object value)
    {
        this.CheckUnarity();

        var greaterThan = new GreaterThan(this.extent, role, value);

        this.extent.Invalidate();
        this.predicate = greaterThan;
        return greaterThan;
    }

    public IPredicate AddInstanceOf(Composite type)
    {
        this.CheckUnarity();

        var instanceOf = new InstanceOfObject(type);

        this.extent.Invalidate();
        this.predicate = instanceOf;
        return instanceOf;
    }

    public IPredicate AddInstanceOf(RoleType role, Composite type)
    {
        this.CheckUnarity();

        var instanceOf = new InstanceOfRole(this.extent, role, type);

        this.extent.Invalidate();
        this.predicate = instanceOf;
        return instanceOf;
    }

    public IPredicate AddInstanceOf(AssociationType association, Composite type)
    {
        this.CheckUnarity();

        var instanceOf = new InstanceOfAssociation(this.extent, association, type);

        this.extent.Invalidate();
        this.predicate = instanceOf;
        return instanceOf;
    }

    public IPredicate AddLessThan(RoleType role, object value)
    {
        this.CheckUnarity();

        var lessThan = new LessThan(this.extent, role, value);

        this.extent.Invalidate();
        this.predicate = lessThan;
        return lessThan;
    }

    public IPredicate AddLike(RoleType role, string value)
    {
        this.CheckUnarity();

        var like = new Like(this.extent, role, value);

        this.extent.Invalidate();
        this.predicate = like;
        return like;
    }

    public ICompositePredicate AddNot(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();

        var not = new Not(this.extent);
        init?.Invoke(not);

        this.extent.Invalidate();
        this.predicate = not;
        return not;
    }

    public ICompositePredicate AddOr(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();

        var or = new Or(this.extent);
        init?.Invoke(or);

        this.extent.Invalidate();
        this.predicate = or;
        return or;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy) =>
        this.predicate.Evaluate(strategy) switch
        {
            ThreeValuedLogic.True => ThreeValuedLogic.False,
            ThreeValuedLogic.False => ThreeValuedLogic.True,
            _ => ThreeValuedLogic.Unknown,
        };

    private void CheckUnarity()
    {
        if (this.predicate != null)
        {
            throw new ArgumentException("Not predicate accepts only 1 operator (unary)");
        }
    }
}

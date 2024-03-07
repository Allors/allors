// <copyright file="Not.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class Not : Predicate, ICompositePredicate
{
    private readonly IInternalExtentFiltered extent;
    private Predicate filter;

    internal Not(IInternalExtentFiltered extent)
    {
        this.extent = extent;
    }

    internal override bool Include => this.filter != null && this.filter.Include;

    public ICompositePredicate AddAnd(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();

        var and = new And(this.extent);
        init?.Invoke(and);

        this.extent.FlushCache();
        this.filter = and;
        return (ICompositePredicate)this.filter;
    }

    public IPredicate AddBetween(RoleType role, object firstValue, object secondValue)
    {
        this.CheckUnarity();

        Between between;
        if (firstValue is RoleType betweenRoleA && secondValue is RoleType betweenRoleB)
        {
            between = new RoleBetweenRole(this.extent, role, betweenRoleA, betweenRoleB);
        }
        else if (firstValue is AssociationType || secondValue is AssociationType)
        {
            throw new NotImplementedException();
        }
        else
        {
            between = new RoleBetweenValue(this.extent, role, firstValue, secondValue);
        }

        this.extent.FlushCache();
        this.filter = between;
        return between;
    }

    public IPredicate AddIn(RoleType role, Allors.Database.IExtent<IObject> containingExtent)
    {
        this.CheckUnarity();

        var containedIn = new RoleInExtent(this.extent, role, containingExtent);

        this.extent.FlushCache();
        this.filter = containedIn;
        return containedIn;
    }

    public IPredicate AddIn(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        this.CheckUnarity();
        
        var containedIn = new NotRoleInEnumerable(this.extent, role, containingEnumerable);

        this.extent.FlushCache();
        this.filter = containedIn;
        return containedIn;
    }

    public IPredicate AddIn(AssociationType association, Allors.Database.IExtent<IObject> containingExtent)
    {
        this.CheckUnarity();

        var containedIn = new NotAssociationInExtent(this.extent, association, containingExtent);

        this.extent.FlushCache();
        this.filter = containedIn;
        return containedIn;
    }

    public IPredicate AddIn(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        this.CheckUnarity();
        
        var containedIn = new NotAssociationInEnumerable(this.extent, association, containingEnumerable);

        this.extent.FlushCache();
        this.filter = containedIn;
        return containedIn;
    }

    public IPredicate AddContains(RoleType role, IObject containedObject)
    {
        this.CheckUnarity();
        
        var contains = new RoleContains(this.extent, role, containedObject);

        this.extent.FlushCache();
        this.filter = contains;
        return contains;
    }

    public IPredicate AddContains(AssociationType association, IObject containedObject)
    {
        this.CheckUnarity();

        var contains = new AssociationContains(this.extent, association, containedObject);

        this.extent.FlushCache();
        this.filter = contains;
        return contains;
    }

    public IPredicate AddEquals(IObject allorsObject)
    {
        this.CheckUnarity();
        
        var equals = new EqualsComposite(allorsObject);

        this.extent.FlushCache();
        this.filter = equals;
        return equals;
    }

    public IPredicate AddEquals(RoleType role, object obj)
    {
        this.CheckUnarity();

        Equals equals;
        if (obj is RoleType equalsRole)
        {
            equals = new RoleEqualsRole(this.extent, role, equalsRole);
        }
        else if (obj is AssociationType equalsAssociation)
        {
            throw new NotImplementedException();
        }
        else
        {
            equals = new RoleEqualsValue(this.extent, role, obj);
        }

        this.extent.FlushCache();
        this.filter = equals;
        return equals;
    }

    public IPredicate AddEquals(AssociationType association, IObject allorsObject)
    {
        this.CheckUnarity();
        
        Equals equals = new AssociationEquals(this.extent, association, allorsObject);
        
        this.extent.FlushCache();
        this.filter = equals;
        return equals;
    }

    public IPredicate AddExists(RoleType role)
    {
        this.CheckUnarity();
        
        var exists = new RoleExists(this.extent, role);

        this.extent.FlushCache();
        this.filter = exists;
        return exists;
    }

    public IPredicate AddExists(AssociationType association)
    {
        this.CheckUnarity();
        
        var exists = new AssociationExists(this.extent, association);

        this.extent.FlushCache();
        this.filter = exists;
        return exists;
    }

    public IPredicate AddGreaterThan(RoleType role, object value)
    {
        this.CheckUnarity();

        GreaterThan greaterThan;
        if (value is RoleType greaterThanRole)
        {
            greaterThan = new RoleGreaterThanRole(this.extent, role, greaterThanRole);
        }
        else if (value is AssociationType greaterThanAssociation)
        {
            throw new NotImplementedException();
        }
        else
        {
            greaterThan = new RoleGreaterThanValue(this.extent, role, value);
        }

        this.extent.FlushCache();
        this.filter = greaterThan;
        return greaterThan;
    }

    public IPredicate AddInstanceOf(Composite type)
    {
        this.CheckUnarity();
        
        var instanceOf = new InstanceOfComposite(type, CompositePredicate.GetConcreteSubClasses(type));

        this.extent.FlushCache();
        this.filter = instanceOf;
        return instanceOf;
    }

    public IPredicate AddInstanceOf(RoleType role, Composite type)
    {
        this.CheckUnarity();
       
        var instanceOf = new RoleInstanceof(this.extent, role, type, CompositePredicate.GetConcreteSubClasses(type));

        this.extent.FlushCache();
        this.filter = instanceOf;
        return instanceOf;
    }

    public IPredicate AddInstanceOf(AssociationType association, Composite type)
    {
        this.CheckUnarity();
        
        var instanceOf = new AssociationInstanceOf(this.extent, association, type, CompositePredicate.GetConcreteSubClasses(type));

        this.extent.FlushCache();
        this.filter = instanceOf;
        return instanceOf;
    }

    public IPredicate AddLessThan(RoleType role, object value)
    {
        this.CheckUnarity();

        LessThan lessThan = value switch
        {
            AssociationType => throw new NotImplementedException(),
            RoleType lessThanRole => new RoleLessThanRole(this.extent, role, lessThanRole),
            _ => new RoleLessThanValue(this.extent, role, value)
        };

        this.extent.FlushCache();
        this.filter = lessThan;
        return lessThan;
    }

    public IPredicate AddLike(RoleType role, string value)
    {
        this.CheckUnarity();

        var like = new Like(this.extent, role, value);

        this.extent.FlushCache();
        this.filter = like;
        return like;
    }

    public ICompositePredicate AddNot(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();
        
        var not = new Not(this.extent);
        init?.Invoke(not);

        this.extent.FlushCache();
        this.filter = not;
        return (ICompositePredicate)this.filter;
    }

    public ICompositePredicate AddOr(Action<ICompositePredicate> init = null)
    {
        this.CheckUnarity();

        var or = new Or(this.extent);
        init?.Invoke(or);

        this.extent.FlushCache();
        this.filter = or;
        return (ICompositePredicate)this.filter;
    }

    internal override bool BuildWhere(ExtentStatement statement, string alias)
    {
        if (this.Include)
        {
            if (this.filter.IsNotFilter)
            {
                this.filter.BuildWhere(statement, alias);
            }
            else
            {
                statement.Append(" NOT (");
                this.filter.BuildWhere(statement, alias);
                statement.Append(")");
            }
        }

        return this.Include;
    }

    internal override void Setup(ExtentStatement statement)
    {
        if (this.filter != null)
        {
            this.filter.Setup(statement);
        }
    }

    private void CheckUnarity()
    {
        if (this.filter != null)
        {
            throw new ArgumentException("Not predicate accepts only 1 operator (unary)");
        }
    }
}

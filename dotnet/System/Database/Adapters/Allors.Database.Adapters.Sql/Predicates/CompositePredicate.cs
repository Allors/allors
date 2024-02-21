// <copyright file="CompositePredicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

internal abstract class CompositePredicate : Predicate, ICompositePredicate
{
    protected CompositePredicate(ExtentFiltered extent)
    {
        this.Extent = extent;
        this.Filters = new List<Predicate>(4);

        if (extent.Strategy != null)
        {
            var allorsObject = extent.Strategy.GetObject();
            if (extent.AssociationType != null)
            {
                var role = extent.AssociationType.RoleType;
                if (role.IsMany)
                {
                    this.AddContains(role, allorsObject);
                }
                else
                {
                    this.AddEquals(role, allorsObject);
                }
            }
            else
            {
                var association = extent.RoleType.AssociationType;
                if (association.IsMany)
                {
                    this.AddContains(association, allorsObject);
                }
                else
                {
                    this.AddEquals(association, allorsObject);
                }
            }
        }
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

    protected ExtentFiltered Extent { get; }

    protected List<Predicate> Filters { get; }

    public ICompositePredicate AddAnd()
    {
        this.Extent.FlushCache();
        var allFilter = new AndPredicate(this.Extent);
        this.Filters.Add(allFilter);
        return allFilter;
    }

    public ICompositePredicate AddBetween(RoleType role, object firstValue, object secondValue)
    {
        this.Extent.FlushCache();
        if (firstValue is RoleType betweenRoleA && secondValue is RoleType betweenRoleB)
        {
            this.Filters.Add(new RoleBetweenRole(this.Extent, role, betweenRoleA, betweenRoleB));
        }
        else if (firstValue is AssociationType betweenAssociationA && secondValue is AssociationType betweenAssociationB)
        {
            throw new NotImplementedException();
        }
        else
        {
            this.Filters.Add(new RoleBetweenValue(this.Extent, role, firstValue, secondValue));
        }

        return this;
    }

    public ICompositePredicate AddContainedIn(RoleType role, Allors.Database.Extent containingExtent)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleContainedInExtent(this.Extent, role, containingExtent));
        return this;
    }

    public ICompositePredicate AddContainedIn(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleContainedInEnumerable(this.Extent, role, containingEnumerable));
        return this;
    }

    public ICompositePredicate AddContainedIn(AssociationType association, Allors.Database.Extent containingExtent)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationContainedInExtent(this.Extent, association, containingExtent));
        return this;
    }

    public ICompositePredicate AddContainedIn(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationContainedInEnumerable(this.Extent, association, containingEnumerable));
        return this;
    }

    public ICompositePredicate AddContains(RoleType role, IObject containedObject)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleContains(this.Extent, role, containedObject));
        return this;
    }

    public ICompositePredicate AddContains(AssociationType association, IObject containedObject)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationContains(this.Extent, association, containedObject));
        return this;
    }

    public ICompositePredicate AddEquals(IObject allorsObject)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new Equals(allorsObject));
        return this;
    }

    public ICompositePredicate AddEquals(RoleType role, object obj)
    {
        this.Extent.FlushCache();
        if (obj is RoleType equalsRole)
        {
            this.Filters.Add(new RoleEqualsRole(this.Extent, role, equalsRole));
        }
        else if (obj is AssociationType equalsAssociation)
        {
            throw new NotImplementedException();
        }
        else
        {
            this.Filters.Add(new RoleEqualsValue(this.Extent, role, obj));
        }

        return this;
    }

    public ICompositePredicate AddEquals(AssociationType association, IObject allorsObject)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationEquals(this.Extent, association, allorsObject));
        return this;
    }

    public ICompositePredicate AddExists(RoleType role)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleExists(this.Extent, role));
        return this;
    }

    public ICompositePredicate AddExists(AssociationType association)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationExists(this.Extent, association));
        return this;
    }

    public ICompositePredicate AddGreaterThan(RoleType role, object value)
    {
        this.Extent.FlushCache();
        if (value is RoleType greaterThanRole)
        {
            this.Filters.Add(new RoleGreaterThanRole(this.Extent, role, greaterThanRole));
        }
        else if (value is AssociationType greaterThanAssociation)
        {
            throw new NotImplementedException();
        }
        else
        {
            this.Filters.Add(new RoleGreaterThanValue(this.Extent, role, value));
        }

        return this;
    }

    public ICompositePredicate AddInstanceof(IComposite type)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new InstanceOf(type, GetConcreteSubClasses(type)));
        return this;
    }

    public ICompositePredicate AddInstanceof(RoleType role, IComposite type)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleInstanceof(this.Extent, role, type, GetConcreteSubClasses(type)));
        return this;
    }

    public ICompositePredicate AddInstanceof(AssociationType association, IComposite type)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new AssociationInstanceOf(this.Extent, association, type, GetConcreteSubClasses(type)));
        return this;
    }

    public ICompositePredicate AddLessThan(RoleType role, object value)
    {
        this.Extent.FlushCache();
        if (value is RoleType lessThanRole)
        {
            this.Filters.Add(new RoleLessThanRole(this.Extent, role, lessThanRole));
        }
        else if (value is AssociationType lessThanAssociation)
        {
            throw new NotImplementedException();
        }
        else
        {
            this.Filters.Add(new RoleLessThanValue(this.Extent, role, value));
        }

        return this;
    }

    public ICompositePredicate AddLike(RoleType role, string value)
    {
        this.Extent.FlushCache();
        this.Filters.Add(new RoleLike(this.Extent, role, value));
        return this;
    }

    public ICompositePredicate AddNot()
    {
        this.Extent.FlushCache();
        var noneFilter = new Not(this.Extent);
        this.Filters.Add(noneFilter);
        return noneFilter;
    }

    public ICompositePredicate AddOr()
    {
        this.Extent.FlushCache();
        var anyFilter = new Or(this.Extent);
        this.Filters.Add(anyFilter);
        return anyFilter;
    }

    internal static IObjectType[] GetConcreteSubClasses(IObjectType type)
    {
        if (type.IsInterface)
        {
            return ((Interface)type).Classes.ToArray();
        }

        var concreteSubclasses = new IObjectType[1];
        concreteSubclasses[0] = type;
        return concreteSubclasses;
    }

    internal override void Setup(ExtentStatement statement)
    {
        foreach (var filter in this.Filters)
        {
            filter.Setup(statement);
        }
    }
}

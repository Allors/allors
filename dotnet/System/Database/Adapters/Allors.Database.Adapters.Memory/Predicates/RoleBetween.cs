// <copyright file="RoleBetween.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using Allors.Database.Meta;

internal sealed class RoleBetween : Predicate
{
    private readonly ExtentFiltered extent;
    private readonly object first;
    private readonly RoleType roleType;
    private readonly object second;

    internal RoleBetween(ExtentFiltered extent, RoleType roleType, object first, object second)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleBetween(roleType, first, second);

        this.extent = extent;
        this.roleType = roleType;

        this.first = first;
        this.second = second;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var firstValue = this.first;
        var secondValue = this.second;

        if (this.first is RoleType firstRole)
        {
            firstValue = strategy.GetInternalizedUnitRole(firstRole);
        }
        else if (this.first is RoleTypeIndex firstRoleIndex)
        {
            firstValue = strategy.GetInternalizedUnitRole(firstRoleIndex.RoleType);
        }
        else if (this.roleType.ObjectType is Unit)
        {
            firstValue = this.roleType.Normalize(this.first);
        }

        if (this.second is RoleType secondRole)
        {
            secondValue = strategy.GetInternalizedUnitRole(secondRole);
        }
        else if (this.second is RoleTypeIndex secondRoleIndex)
        {
            secondValue = strategy.GetInternalizedUnitRole(secondRoleIndex.RoleType);
        }
        else if (this.roleType.ObjectType is Unit)
        {
            secondValue = this.roleType.Normalize(this.second);
        }

        if (!(strategy.GetInternalizedUnitRole(this.roleType) is IComparable comparable))
        {
            return ThreeValuedLogic.Unknown;
        }

        return comparable.CompareTo(firstValue) >= 0 && comparable.CompareTo(secondValue) <= 0
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

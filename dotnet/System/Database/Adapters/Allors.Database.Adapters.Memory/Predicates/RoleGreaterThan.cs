// <copyright file="RoleGreaterThan.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsPredicateRoleGreaterThanValueMemory type.
// </summary>

namespace Allors.Database.Adapters.Memory;

using System;
using Allors.Database.Meta;

internal sealed class RoleGreaterThan : Predicate
{
    private readonly object compare;
    private readonly ExtentFiltered extent;
    private readonly IRoleType roleType;

    internal RoleGreaterThan(ExtentFiltered extent, IRoleType roleType, object compare)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleGreaterThan(roleType, compare);

        this.extent = extent;
        this.roleType = roleType;
        this.compare = compare;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var compareValue = this.compare;

        if (this.compare is IRoleType compareRole)
        {
            compareValue = strategy.GetInternalizedUnitRole(compareRole);
        }
        else if (this.roleType.ObjectType is IUnit)
        {
            compareValue = this.roleType.Normalize(this.compare);
        }

        if (!(strategy.GetInternalizedUnitRole(this.roleType) is IComparable comparable))
        {
            return ThreeValuedLogic.Unknown;
        }

        return comparable.CompareTo(compareValue) > 0
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

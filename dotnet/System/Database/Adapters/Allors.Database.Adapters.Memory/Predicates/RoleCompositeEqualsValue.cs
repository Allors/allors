// <copyright file="RoleCompositeEquals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class RoleCompositeEqualsValue : Predicate
{
    private readonly object equals;
    private readonly RoleType roleType;

    internal RoleCompositeEqualsValue(ExtentFiltered extent, RoleType roleType, object equals)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleEquals(roleType, equals);

        this.roleType = roleType;
        this.equals = equals;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        object value = strategy.GetCompositeRole(this.roleType);

        if (value == null)
        {
            return ThreeValuedLogic.False;
        }

        var equalsValue = this.equals;

        if (this.equals is RoleType)
        {
            var equalsRole = (RoleType)this.equals;
            equalsValue = strategy.GetCompositeRole(equalsRole);
        }

        if (equalsValue == null)
        {
            return ThreeValuedLogic.False;
        }

        return value.Equals(equalsValue)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

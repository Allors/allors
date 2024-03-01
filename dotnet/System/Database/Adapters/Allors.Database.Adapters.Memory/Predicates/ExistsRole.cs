// <copyright file="RoleExists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class ExistsRole : Exists
{
    private readonly RoleType roleType;

    internal ExistsRole(ExtentFiltered extent, RoleType roleType)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleExists(roleType);

        this.roleType = roleType;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy) =>
        strategy.ExistRole(this.roleType) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
}

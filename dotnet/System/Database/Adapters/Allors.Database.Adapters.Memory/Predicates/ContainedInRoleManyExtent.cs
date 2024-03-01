// <copyright file="RoleManyContainedInExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class ContainedInRoleManyExtent : ContainedIn
{
    private readonly Allors.Database.Extent containingExtent;
    private readonly RoleType roleType;

    internal ContainedInRoleManyExtent(ExtentFiltered extent, RoleType roleType, Allors.Database.Extent containingExtent)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleContainedIn(roleType, containingExtent);

        this.roleType = roleType;
        this.containingExtent = containingExtent;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy) =>
        strategy.GetCompositesRole<IObject>(this.roleType).Any(role => this.containingExtent.Contains(role))
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
}

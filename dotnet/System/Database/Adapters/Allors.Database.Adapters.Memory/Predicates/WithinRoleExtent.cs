// <copyright file="InRoleOneExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class WithinRoleExtent : Within
{
    private readonly IExtent<IObject> containingExtent;
    private readonly RoleType roleType;

    internal WithinRoleExtent(IInternalExtent extent, RoleType roleType, IExtent<IObject> containingExtent)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleIn(roleType, containingExtent);

        this.roleType = roleType;
        this.containingExtent = containingExtent;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var roleStrategy = strategy.GetCompositeRole(this.roleType);

        if (roleStrategy == null)
        {
            return ThreeValuedLogic.False;
        }

        return this.containingExtent.Contains(roleStrategy)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

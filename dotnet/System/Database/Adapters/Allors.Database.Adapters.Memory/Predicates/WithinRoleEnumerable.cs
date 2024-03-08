// <copyright file="InRoleOneEnumerable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class WithinRoleEnumerable : Within
{
    private readonly IEnumerable<IObject> containingEnumerable;
    private readonly RoleType roleType;

    internal WithinRoleEnumerable(IInternalExtent extent, RoleType roleType, IEnumerable<IObject> containingEnumerable)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleIn(roleType, containingEnumerable);

        this.roleType = roleType;
        this.containingEnumerable = containingEnumerable;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var containing = new HashSet<IObject>(this.containingEnumerable);
        var roleStrategy = strategy.GetCompositeRole(this.roleType);

        if (roleStrategy == null)
        {
            return ThreeValuedLogic.False;
        }

        return containing.Contains(roleStrategy)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

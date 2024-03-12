// <copyright file="RoleContains.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class HasRole : Has
{
    private readonly IObject containedObject;
    private readonly RoleType roleType;

    internal HasRole(IInternalExtent extent, RoleType roleType, IObject containedObject)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleContains(roleType, containedObject);

        this.roleType = roleType;
        this.containedObject = containedObject;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy) =>
        strategy.GetCompositesRole<IObject>(this.roleType).Contains(this.containedObject) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
}

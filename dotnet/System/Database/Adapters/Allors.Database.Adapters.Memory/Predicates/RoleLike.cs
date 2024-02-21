// <copyright file="RoleLike.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Text.RegularExpressions;
using Allors.Database.Meta;

internal sealed class RoleLike : Predicate
{
    private readonly bool isEmpty;
    private readonly Regex regex;
    private readonly RoleType roleType;

    internal RoleLike(ExtentFiltered extent, RoleType roleType, string like)
    {
        extent.CheckForRoleType(roleType);
        PredicateAssertions.ValidateRoleLikeFilter(roleType, like);

        this.roleType = roleType;
        this.isEmpty = like.Length == 0;
        this.regex = new Regex("^" + like.Replace("%", ".*") + "$");
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var value = (string)strategy.GetInternalizedUnitRole(this.roleType);

        if (value == null)
        {
            return ThreeValuedLogic.Unknown;
        }

        if (this.isEmpty)
        {
            return ThreeValuedLogic.False;
        }

        return this.regex.Match(value).Success
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

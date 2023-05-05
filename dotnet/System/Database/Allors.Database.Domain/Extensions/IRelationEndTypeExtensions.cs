// <copyright file="IRelationEndTypeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

public static class IRelationEndTypeExtensions
{
    public static object Get(this IRelationEndType @this, IStrategy strategy, IComposite ofType = null)
    {
        if (@this is IRoleType roleType)
        {
            return roleType.Get(strategy, ofType);
        }

        var associationType = (IAssociationType)@this;
        return associationType.Get(strategy, ofType);
    }
}

// <copyright file="IRelationEndTypeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

public static class IRelationEndTypeExtensions
{
    public static object Get(this RelationEndType @this, IStrategy strategy, IComposite ofType = null)
    {
        if (@this is RoleType roleType)
        {
            return roleType.Get(strategy, ofType);
        }

        var associationType = (AssociationType)@this;
        return associationType.Get(strategy, ofType);
    }
}

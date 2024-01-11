// <copyright file="IRoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

using System.Linq;

namespace Allors.Database.Meta;

using System.Collections.Generic;

public static class IRoleTypeExtensions
{
    public static object Get(this IRoleType @this, IStrategy strategy, IComposite ofType = null)
    {
        var role = strategy.GetRole(@this);

        if (ofType == null || role == null || !@this.ObjectType.IsComposite)
        {
            return role;
        }

        if (@this.IsOne)
        {
            return ofType.IsAssignableFrom(((IObject)role).Strategy.Class) ? role : null;
        }

        var extent = (IEnumerable<IObject>)role;
        return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
    }

    public static void Set(this IRoleType @this, IStrategy strategy, object value)
    {
        strategy.SetRole(@this, value);
    }
}

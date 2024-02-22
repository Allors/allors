// <copyright file="IRelationEndTypeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

using System.Collections.Generic;
using System.Linq;

namespace Allors.Database.Meta;

public static class IAssociationTypeExtensions
{
    public static object Get(this AssociationType @this, IStrategy strategy, Composite ofType = null)
    {
        var association = strategy.GetAssociation(@this);

        if (ofType == null || association == null)
        {
            return association;
        }

        if (@this.IsMany)
        {
            var extent = (IEnumerable<IObject>)association;
            return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
        }

        return !ofType.IsAssignableFrom(((IObject)association).Strategy.Class) ? null : association;
    }
}

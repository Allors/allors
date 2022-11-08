// <copyright file="PrefetchRule.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>

namespace Allors.Database;

using System;
using Allors.Database.Meta;

public sealed class PrefetchRule
{
    public PrefetchRule(IRelationEndType relationEndType, PrefetchPolicy prefetchPolicy)
    {
        if (relationEndType == null)
        {
            throw new ArgumentNullException("relationEndType");
        }

        if (prefetchPolicy != null && relationEndType is IRoleType roleType && roleType.ObjectType.IsUnit)
        {
            throw new ArgumentException("prefetchPolicy");
        }

        this.RelationEndType = relationEndType;
        this.PrefetchPolicy = prefetchPolicy;
    }

    public IRelationEndType RelationEndType { get; }

    public PrefetchPolicy PrefetchPolicy { get; }

    public override string ToString() => this.RelationEndType.ToString();
}

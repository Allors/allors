// <copyright file="PrefetchPolicyBuilder.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectIdInteger type.</summary>

namespace Allors.Database;

using System.Collections.Generic;
using Allors.Database.Meta;

public sealed class PrefetchPolicyBuilder
{
    private bool allowCompilation = true;
    private List<PrefetchRule> rules = new();

    public PrefetchPolicyBuilder WithRule(RelationEndType relationEndType)
    {
        var rule = new PrefetchRule(relationEndType, null);
        this.rules.Add(rule);
        return this;
    }

    public PrefetchPolicyBuilder WithRule(RelationEndType relationEndType, PrefetchPolicy prefetch)
    {
        var rule = new PrefetchRule(relationEndType, prefetch);
        this.rules.Add(rule);
        return this;
    }

    public PrefetchPolicyBuilder WithAllowCompilation(bool allowCompilation)
    {
        this.allowCompilation = allowCompilation;
        return this;
    }

    public PrefetchPolicy Build()
    {
        try
        {
            return new PrefetchPolicy([.. this.rules]) { AllowCompilation = this.allowCompilation };
        }
        finally
        {
            this.rules = null;
        }
    }
}

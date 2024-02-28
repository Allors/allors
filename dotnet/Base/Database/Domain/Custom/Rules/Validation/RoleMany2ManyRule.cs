// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;

    public class RoleMany2ManyRule : Rule<AA, AAIndex>
    {
        public RoleMany2ManyRule(IMetaIndex m) : base(m, m.AA, new Guid("4383159F-258D-4FCB-833C-55D2B91109A1")) =>
            this.Patterns =
            [
                this.Builder.Pattern<CC>(m.CC.Assigned, v=>v.BBsWhereMany2Many.SelectMany(w=>w.AAsWhereMany2Many)),
                this.Builder.Pattern<CC>(m.CC.Assigned, v=>v.BBsWhereUnusedMany2Many.SelectMany(w=>w.AAsWhereUnusedMany2Many)),
            ];

        public override void Derive(ICycle cycle, IEnumerable<AA> matches)
        {
            foreach (var aa in matches)
            {
                aa.Derived = aa.Many2Many.FirstOrDefault()?.Many2Many.FirstOrDefault()?.Assigned;
            }
        }
    }
}

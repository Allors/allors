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

    public class RoleMany2OneRule : Rule<AA, AAIndex>
    {
        public RoleMany2OneRule(IMetaIndex m) : base(m, m.AA, new Guid("cbebe35e-9931-4701-8b05-8ed61b266bb2")) =>
            this.Patterns =
            [
                this.Builder.Pattern<CC>(m.CC.Assigned, v=>v.BBsWhereMany2One.SelectMany(w=>w.AAsWhereMany2One)),
                this.Builder.Pattern<CC>(m.CC.Assigned, v=>v.BBsWhereUnusedMany2One.SelectMany(w=>w.AAsWhereUnusedMany2One)),
            ];

        public override void Derive(ICycle cycle, IEnumerable<AA> matches)
        {
            foreach (var aa in matches)
            {
                aa.Derived = aa.Many2One?.Many2One?.Assigned;
            }
        }
    }
}

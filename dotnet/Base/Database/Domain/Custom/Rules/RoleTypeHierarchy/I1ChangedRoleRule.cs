// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;

    public class I1ChangedRoleRule : Rule<I1, I1Index>
    {
        public I1ChangedRoleRule(IMetaIndex m) : base(m, m.I1, new Guid("475E8B38-21BB-40F9-AD67-9A7432F73CDD")) =>
            this.Patterns =
            [
                this.Builder.Pattern<S12>(m.S12.ChangedRolePingI1, v=> v as I1),
            ];

        public override void Derive(ICycle cycle, IEnumerable<I1> matches)
        {
            foreach (var i1 in matches)
            {
                i1.ChangedRolePongI1 = i1.ChangedRolePingI1;
            }
        }
    }
}

﻿// <copyright file="Domain.cs" company="Allors bv">
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
    using Allors.Database.Meta;

    public class RoleOne2OneRule : Rule<AA>
    {
        public RoleOne2OneRule(IMetaIndex m) : base(m, new Guid("1C369F4C-CC12-4064-9261-BF899205E251")) =>
            this.Patterns = new[]
            {
                new RolePattern<CC, AA>(m.CC.Assigned, v=>v.BBWhereOne2One.AAWhereOne2One),
                new RolePattern<CC, AA>(m.CC.Assigned, v=>v.BBWhereUnusedOne2One.AAWhereUnusedOne2One),
            };

        public override void Derive(ICycle cycle, IEnumerable<AA> matches)
        {
            foreach (var aa in matches)
            {
                aa.Derived = aa.One2One?.One2One?.Assigned;
            }
        }
    }
}

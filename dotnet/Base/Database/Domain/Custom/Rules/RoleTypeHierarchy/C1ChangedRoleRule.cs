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

    public class C1ChangedRoleRule : Rule<C1>
    {
        public C1ChangedRoleRule(IMetaIndex m) : base(m, new Guid("84343F1E-7224-41CE-9B4C-69883417115F")) =>
            this.Patterns = new[]
            {
                new RolePattern<S12, C1>(m.S12.ChangedRolePingC1, v=> v as C1) ,
            };

        public override void Derive(ICycle cycle, IEnumerable<C1> matches)
        {
            foreach (var c1 in matches)
            {
                c1.ChangedRolePongC1 = c1.ChangedRolePingC1;
            }
        }
    }
}

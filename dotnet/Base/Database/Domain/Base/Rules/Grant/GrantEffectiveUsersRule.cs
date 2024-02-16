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

    public class GrantEffectiveUsersRule : Rule
    {
        public GrantEffectiveUsersRule(M m) : base(m, new Guid("2D3F4F02-7439-48E7-9E5B-363F4A4384F0")) =>
            this.Patterns =
            [
                m.Grant.RolePattern(v=>v.Subjects),
                m.Grant.RolePattern(v=>v.SubjectGroups),
                m.UserGroup.RolePattern(v=>v.Members, v=>v.GrantsWhereSubjectGroup),
            ];

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var grant in matches.Cast<Grant>())
            {
                grant.EffectiveUsers = grant
                    .SubjectGroups.SelectMany(v => v.Members)
                    .Union(grant.Subjects)
                    .ToArray();
            }
        }
    }
}

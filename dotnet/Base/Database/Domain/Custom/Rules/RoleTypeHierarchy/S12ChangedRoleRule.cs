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
    using Allors.Database.Meta;

    public class S12ChangedRoleRule : Rule
    {
        public S12ChangedRoleRule(MetaIndex m) : base(m, new Guid("68E9CC01-5DC2-466F-AA2A-2B9F337C2D2E")) =>
            this.Patterns =
            [
                new RolePattern(m.S12.ChangedRolePingS12, m.S12),
            ];


        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var s12 in matches.Cast<S12>())
            {
                s12.ChangedRolePongS12 = s12.ChangedRolePingS12;
            }
        }
    }
}

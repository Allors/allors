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
public class S12ChangedRoleRule : Rule<S12>
    {
        public S12ChangedRoleRule(IMetaIndex m) : base(m, new Guid("68E9CC01-5DC2-466F-AA2A-2B9F337C2D2E")) =>
            this.Patterns =
            [
                new RolePattern<S12, S12>(m.S12.ChangedRolePingS12),
            ];
        
        public override void Derive(ICycle cycle, IEnumerable<S12> matches)
        {
            foreach (var s12 in matches)
            {
                s12.ChangedRolePongS12 = s12.ChangedRolePingS12;
            }
        }
    }
}

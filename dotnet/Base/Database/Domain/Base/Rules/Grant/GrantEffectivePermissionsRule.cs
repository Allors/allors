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

    public class GrantEffectivePermissionsRule : Rule<Grant>
    {
        public GrantEffectivePermissionsRule(IMetaIndex m) : base(m, new Guid("1F897B84-EF92-4E94-8877-3501D56D426B"))
        {
            //var tree = new GrantTreeBuilder
            //{
            //    Role = new(m)
            //    {
            //        Role = new()
            //        {
            //            Permissions = new(m)
            //        }
            //    }
            //};

            this.Patterns =
            [
                new RolePattern<Grant, Grant>(m.Grant.Role),
                new RolePattern<Role, Grant>(m.Role.Permissions, v=>v.GrantsWhereRole),
            ];
        }

        public override void Derive(ICycle cycle, IEnumerable<Grant> matches)
        {
            foreach (var grant in matches)
            {
                grant.EffectivePermissions = grant.Role?.Permissions.ToArray();
            }
        }
    }
}

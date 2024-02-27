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

    public class UserNormalizedUserEmailRule : Rule<User>
    {
        public UserNormalizedUserEmailRule(IMetaIndex m) : base(m, new Guid("904187C3-773E-47BC-A2EA-EF45ECA78FD2")) =>
               this.Patterns =
               [
                   new RolePattern<User, User>(m.User.UserEmail),
               ];

        public override void Derive(ICycle cycle, IEnumerable<User> matches)
        {
            foreach (var @this in matches)
            {
                @this.NormalizedUserEmail = User.Normalize(@this.UserEmail);
            }
        }
    }
}

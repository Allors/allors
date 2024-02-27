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

    public class UserInUserPasswordRule : Rule<User>
    {
        public UserInUserPasswordRule(IMetaIndex m) : base(m, new Guid("AF93DA46-1C9A-47C4-9E5F-6A04751F5259")) =>
            this.Patterns =
            [
                new RolePattern<User, User>(m.User.InExistingUserPassword),
                new RolePattern<User, User>(m.User.InUserPassword),
            ];

        public override void Derive(ICycle cycle, IEnumerable<User> matches)
        {
            foreach (var @this in matches)
            {
                var passwordHasher = @this.Transaction().Database.Services.Get<IPasswordHasher>();
                var m = @this.Transaction().Database.Services.Get<IMetaIndex>();

                try
                {
                    if (!string.IsNullOrWhiteSpace(@this.InExistingUserPassword) && !passwordHasher.VerifyHashedPassword(@this.UserName, @this.UserPasswordHash, @this.InExistingUserPassword))
                    {
                        cycle.Validation.AddError(@this, m.User.InExistingUserPassword, ErrorCodes.InvalidPassword);
                        continue;
                    }

                    if (@this.ExistInUserPassword && !passwordHasher.CheckStrength(@this.InUserPassword))
                    {
                        cycle.Validation.AddError(@this, m.User.InUserPassword, ErrorCodes.InvalidNewPassword);
                        continue;
                    }

                    if (@this.ExistInUserPassword)
                    {
                        @this.UserPasswordHash = passwordHasher.HashPassword(@this.UserName, @this.InUserPassword);
                    }
                }
                finally
                {
                    @this.RemoveInExistingUserPassword();
                    @this.RemoveInUserPassword();
                }
            }
        }
    }
}

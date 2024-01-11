﻿// <copyright file="Auditable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Services;

    public static class AuditableExtensions
    {
        public static void BaseOnPostDerive(this Auditable @this, ObjectOnPostDerive method)
        {
            var user = @this.Transaction().Services.Get<IUserService>().User;
            if (user != null)
            {
                var derivation = method.Derivation;
                var changeSet = derivation.ChangeSet;

                if (changeSet.Created.Contains(@this))
                {
                    @this.CreationDate = @this.Transaction().Now();
                    @this.CreatedBy = (User)user;
                }

                if (changeSet.Associations.Contains(@this))
                {
                    @this.LastModifiedDate = @this.Transaction().Now();
                    @this.LastModifiedBy = (User)user;
                }
            }
        }
    }
}

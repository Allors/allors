﻿// <copyright file="TaskAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class TaskAssignment
    {
        public void BaseOnPostDerive(ObjectOnPostDerive _)
        {
            if (!this.ExistSecurityTokens)
            {
                var cache = this.Transaction().Caches().SecurityTokenByUniqueId();
                var defaultSecurityToken = cache[SecurityToken.DefaultSecurityTokenId];
                this.SecurityTokens = new[] { defaultSecurityToken, this.User?.OwnerSecurityToken };
            }
        }

        public void BaseDelete(DeletableDelete _) => this.Notification?.CascadingDelete();
    }
}

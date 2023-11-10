﻿// <copyright file="NotificationList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class NotificationList
    {
        public void BaseOnPostDerive(ObjectOnPostDerive _)
        {
            if (!this.ExistSecurityTokens && this.ExistUserWhereNotificationList)
            {
                var cache = this.Transaction().Caches().SecurityTokenByUniqueId();
                var defaultSecurityToken = cache[SecurityToken.DefaultSecurityTokenId];
                this.SecurityTokens = new[] { this.UserWhereNotificationList.OwnerSecurityToken, defaultSecurityToken };
            }
        }
    }
}

﻿// <copyright file="NotificationList.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
                var cache = this.Transaction().Scoped<SecurityTokenByUniqueId>();
                var defaultSecurityToken = cache.DefaultSecurityToken;
                this.SecurityTokens = new[] { this.UserWhereNotificationList.OwnerSecurityToken, defaultSecurityToken };
            }
        }
    }
}

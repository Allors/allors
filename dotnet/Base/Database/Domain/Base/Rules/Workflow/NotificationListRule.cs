// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Derivations.Rules;
    using Meta;

    public class NotificationListRule : Rule<NotificationList>
    {
        public NotificationListRule(IMetaIndex m) : base(m, new Guid("e8071e5b-18a4-4a52-8b22-09a75c3dbf72")) =>
            this.Patterns =
            [
                new RolePattern<NotificationList, NotificationList>(m.NotificationList.Notifications),
                new RolePattern<Notification, NotificationList>(m.Notification.Confirmed, v=>v.NotificationListWhereNotification),
                new RolePattern<Notification, NotificationList>(m.Notification.Confirmed, v=>v.NotificationListWhereUnconfirmedNotification),
                new RolePattern<Notification, NotificationList>(m.Notification.Confirmed, v=>v.NotificationListWhereConfirmedNotification),
            ];

        public override void Derive(ICycle cycle, IEnumerable<NotificationList> matches)
        {
            foreach (var @this in matches)
            {
                @this.UnconfirmedNotifications = @this.Notifications.Where(notification => !notification.Confirmed).ToArray();
                @this.ConfirmedNotifications = @this.Notifications.Where(notification => notification.Confirmed).ToArray();

                if (!@this.ExistSecurityTokens)
                {
                    if (@this.ExistUserWhereNotificationList)
                    {
                        var cache = cycle.Transaction.Scoped<SecurityTokenByUniqueId>();
                        var defaultSecurityToken = cache.DefaultSecurityToken;
                        @this.SecurityTokens = [@this.UserWhereNotificationList.OwnerSecurityToken, defaultSecurityToken];
                    }
                }
            }
        }
    }
}

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

    public class NotificationListRule : Rule<NotificationList, NotificationListIndex>
    {
        public NotificationListRule(IMetaIndex m) : base(m, m.NotificationList, new Guid("e8071e5b-18a4-4a52-8b22-09a75c3dbf72")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.Notifications),
                this.Builder.Pattern<Notification>(m.Notification.Confirmed, v=>v.NotificationListWhereNotification),
                this.Builder.Pattern<Notification>(m.Notification.Confirmed, v=>v.NotificationListWhereUnconfirmedNotification),
                this.Builder.Pattern<Notification>(m.Notification.Confirmed, v=>v.NotificationListWhereConfirmedNotification),
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

﻿// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PaymentStateByUniqueId : IScoped
    {
        private readonly ICache<Guid, PaymentState> cache;

        public PaymentStateByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().PaymentStateByUniqueId();
        }

        public PaymentState Unpaid => this.cache[PaymentState.UnpaidId];

        public PaymentState PartiallyPaid => this.cache[PaymentState.PartiallyPaidId];

        public PaymentState Paid => this.cache[PaymentState.PaidId];
    }
}

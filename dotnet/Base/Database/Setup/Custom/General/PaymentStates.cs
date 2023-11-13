// <copyright file="PaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    public partial class PaymentStates
    {
        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().PaymentStateByUniqueId().Merger().Action();

            merge(PaymentState.UnpaidId, v => v.Name = "Unpaid");
            merge(PaymentState.PartiallyPaidId, v => v.Name = "PartiallyPaid");
            merge(PaymentState.PaidId, v => v.Name = "Paid");
        }
    }
}

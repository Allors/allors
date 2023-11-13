// <copyright file="OrderStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    public partial class OrderStates
    {
        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().OrderStateByUniqueId().Merger().Action();

            merge(OrderState.InitialId, v => v.Name = "Initial");
            merge(OrderState.ConfirmedId, v => v.Name = "Confirmed");
            merge(OrderState.ClosedId, v => v.Name = "Closed");
            merge(OrderState.CancelledId, v => v.Name = "Cancelled");
        }
    }
}

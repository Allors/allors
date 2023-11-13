// <copyright file="ShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    public partial class ShipmentStates
    {
        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().ShipmentStateByUniqueId().Merger().Action();

            merge(ShipmentState.NotShippedId, v => v.Name = "NotShipped");
            merge(ShipmentState.PartiallyShippedId, v => v.Name = "PartiallyShipped");
            merge(ShipmentState.ShippedId, v => v.Name = "Shipped");
        }
    }
}

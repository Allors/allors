// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class ShipmentStateByUniqueId
    {
        public ShipmentState NotShipped => this.cache[ShipmentState.NotShippedId];

        public ShipmentState PartiallyShipped => this.cache[ShipmentState.PartiallyShippedId];

        public ShipmentState Shipped => this.cache[ShipmentState.ShippedId];
    }
}

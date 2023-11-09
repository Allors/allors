// <copyright file="OrderStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    using System;
   

    public partial class OrderStates
    {
        private UniquelyIdentifiableCache<OrderState> cache;

        public Cache<Guid, OrderState> Cache => this.cache ??= new UniquelyIdentifiableCache<OrderState>(this.Transaction);

        public OrderState Initial => this.Cache[OrderState.InitialId];

        public OrderState Confirmed => this.Cache[OrderState.ConfirmedId];

        public OrderState Closed => this.Cache[OrderState.ClosedId];

        public OrderState Cancelled => this.Cache[OrderState.CancelledId];

        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(OrderState.InitialId, v => v.Name = "Initial");
            merge(OrderState.ConfirmedId, v => v.Name = "Confirmed");
            merge(OrderState.ClosedId, v => v.Name = "Closed");
            merge(OrderState.CancelledId, v => v.Name = "Cancelled");
        }
    }
}

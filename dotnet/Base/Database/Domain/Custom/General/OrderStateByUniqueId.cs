// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class OrderStateByUniqueId
    {
      public OrderState Initial => this.cache[OrderState.InitialId];

        public OrderState Confirmed => this.cache[OrderState.ConfirmedId];

        public OrderState Closed => this.cache[OrderState.ClosedId];

        public OrderState Cancelled => this.cache[OrderState.CancelledId];
    }
}

// <copyright file="TransactionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public static partial class ICachesExtensions
    {
        public static ICache<Guid, Gender> GenderByUniqueId(this ICaches @this) => @this.Get<Guid, Gender>(@this.M.Gender, @this.M.Gender.UniqueId);

        public static ICache<Guid, OrderState> OrderStateByUniqueId(this ICaches @this) => @this.Get<Guid, OrderState>(@this.M.OrderState, @this.M.OrderState.UniqueId);

        public static ICache<Guid, Organisation> OrganisationByUniqueId(this ICaches @this) => @this.Get<Guid, Organisation>(@this.M.Organisation, @this.M.Organisation.UniqueId);

        public static ICache<Guid, Page> PageByUniqueId(this ICaches @this) => @this.Get<Guid, Page>(@this.M.Page, @this.M.Page.UniqueId);

        public static ICache<Guid, PaymentState> PaymentStateByUniqueId(this ICaches @this) => @this.Get<Guid, PaymentState>(@this.M.PaymentState, @this.M.PaymentState.UniqueId);

        public static ICache<Guid, ShipmentState> ShipmentStateByUniqueId(this ICaches @this) => @this.Get<Guid, ShipmentState>(@this.M.ShipmentState, @this.M.ShipmentState.UniqueId);
    }
}

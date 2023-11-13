// <copyright file="PaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class PaymentState
    {
        public static readonly Guid UnpaidId = new Guid("FC38E48D-C8C4-4F26-A8F1-5D4E962B6F93");
        public static readonly Guid PartiallyPaidId = new Guid("1801737F-2760-4600-9243-7E6BDD8A224D");
        public static readonly Guid PaidId = new Guid("04FAD96A-2B0F-4F07-ABB7-57657A34E422");
    }
}

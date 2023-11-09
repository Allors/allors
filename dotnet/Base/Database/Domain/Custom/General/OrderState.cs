// <copyright file="Order.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class OrderState
    {
        public static readonly Guid InitialId = new Guid("173EF3AF-B5AC-4610-8AC1-916D2C09C1D1");
        public static readonly Guid ConfirmedId = new Guid("BD2FF235-301A-445A-B794-5D76B86006B3");
        public static readonly Guid ClosedId = new Guid("0750D8B3-3B10-465F-BBC0-81D12F40A3DF");
        public static readonly Guid CancelledId = new Guid("F72CEBEE-D12C-4321-83A3-77019A7B8C76");
    }
}

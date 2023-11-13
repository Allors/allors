// <copyright file="ShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class ShipmentState
    {
        public static readonly Guid NotShippedId = new Guid("C74EBE0F-5A3E-4160-9A34-68DC2C69E8B6");
        public static readonly Guid PartiallyShippedId = new Guid("5FF39A43-EFD8-4660-A7E8-60A519BF4C74");
        public static readonly Guid ShippedId = new Guid("B9C74F0B-0FED-4AEF-B087-8062708DCF5F");
    }
}

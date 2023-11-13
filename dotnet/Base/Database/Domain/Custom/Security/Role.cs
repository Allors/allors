// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class Role
    {
        public static readonly Guid OperationsId = new Guid("387E5E5A-727F-4098-9FDC-3431C258E1AA");
        public static readonly Guid ProcurementId = new Guid("ACB4E8EE-61CC-48AA-BB0A-75B279A03049");
        public static readonly Guid SalesId = new Guid("052F86E8-D40D-43CC-9555-9C3107500116");
    }
}

// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Allors.Database.Security;

    public partial class Grant
    {
        public static readonly Guid SalesId = new Guid("9DD281CA-E699-4A2E-8C4F-BCA6CC7B227F");
        public static readonly Guid OperationsId = new Guid("88F6061E-9677-4AA1-ACAC-D7972D527941");
        public static readonly Guid ProcurementId = new Guid("91083059-28D5-419E-B47D-D88E7A621D54");
    }
}

// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class UserGroup
    {
        public static readonly Guid OperationsId = new Guid("4EA028A4-57C6-46A1-AC4B-E18204F9B498");
        public static readonly Guid SalesId = new Guid("1511E4E2-829F-4133-8824-B94ED46E6BED");
        public static readonly Guid ProcurementId = new Guid("FF887B58-CDA3-4C76-8308-0F005E362E0E");
    }
}

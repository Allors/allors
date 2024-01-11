// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class GrantByUniqueId
    {
        public Grant Sales => this.cache[Grant.SalesId];

        public Grant Operations => this.cache[Grant.OperationsId];

        public Grant Procurement => this.cache[Grant.ProcurementId];
    }
}

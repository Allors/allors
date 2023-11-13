// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class RoleByUniqueId 
    {
        public Role Operations => this.cache[Role.OperationsId];

        public Role Procurement => this.cache[Role.ProcurementId];

        public Role Sales => this.cache[Role.SalesId];
    }
}

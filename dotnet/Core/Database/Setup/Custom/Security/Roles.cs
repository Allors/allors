// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    public partial class Roles
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().RoleByUniqueId().Merger().Action();

            merge(Role.OperationsId, v => v.Name = "Operations");
            merge(Role.ProcurementId, v => v.Name = "Procurement");
            merge(Role.SalesId, v => v.Name = "Sales");
        }
    }
}

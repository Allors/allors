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

            this.Transaction.Build<Role>(v =>
            {
                v.Name = "Operations";
                v.UniqueId = Role.OperationsId;
            });

            this.Transaction.Build<Role>(v =>
            {
                v.Name = "Procurement";
                v.UniqueId = Role.ProcurementId;
            });

            this.Transaction.Build<Role>(v =>
            {
                v.Name = "Sales";
                v.UniqueId = Role.SalesId;
            });
        }
    }
}

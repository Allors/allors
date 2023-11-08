// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class Roles
    {
        public Role Operations => this.Cache[Role.OperationsId];

        public Role Procurement => this.Cache[Role.ProcurementId];

        public Role Sales => this.Cache[Role.SalesId];

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Cache.Merger().Action();

            merge(Role.OperationsId, v => v.Name = "Operations");
            merge(Role.ProcurementId, v => v.Name = "Procurement");
            merge(Role.SalesId, v => v.Name = "Sales");
        }
    }
}

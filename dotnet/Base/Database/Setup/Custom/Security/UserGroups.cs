// <copyright file="UserGroups.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    public partial class UserGroups
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().UserGroupByUniqueId().Merger().Action();

            merge(UserGroup.OperationsId, v => v.Name = "operations");
            merge(UserGroup.SalesId, v => v.Name = "sales");
            merge(UserGroup.ProcurementId, v => v.Name = "procurement");
        }
    }
}

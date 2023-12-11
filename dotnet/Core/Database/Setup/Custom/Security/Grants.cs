// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Grants
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            if (setup.Config.SetupSecurity)
            {
                var merge = this.Transaction.Caches().GrantByUniqueId().Merger().Action();

                var roles = this.Transaction.Scoped<RoleByUniqueId>();
                var userGroups = this.Transaction.Scoped<UserGroupByUniqueId>();

                merge(Grant.SalesId, v =>
                  {
                      v.Role = roles.Creator;
                      v.AddSubjectGroup(userGroups.Sales);
                  });

                merge(Grant.OperationsId, v =>
                  {
                      v.Role = roles.Creator;
                      v.AddSubjectGroup(userGroups.Operations);
                  });

                merge(Grant.ProcurementId, v =>
                  {
                      v.Role = roles.Administrator;
                      v.AddSubjectGroup(userGroups.Procurement);
                  });
            }
        }
    }
}

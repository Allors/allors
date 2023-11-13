// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class Grants
    {
        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, this.M.Role);
            setup.AddDependency(this.ObjectType, this.M.UserGroup);
        }

        protected override void CoreSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                var merge = this.Transaction.Caches().GrantByUniqueId().Merger().Action();

                var roles = this.Transaction.Scoped<RoleByUniqueId>();
                var userGroups = this.Transaction.Scoped<UserGroupByUniqueId>();

                merge(Grant.CreatorsId, v =>
                  {
                      v.Role = roles.Creator;
                      v.AddSubjectGroup(userGroups.Creators);
                  });

                merge(Grant.GuestCreatorsId, v =>
                  {
                      v.Role = roles.GuestCreator;
                      v.AddSubjectGroup(userGroups.Guests);
                  });

                merge(Grant.AdministratorId, v =>
                  {
                      v.Role = roles.Administrator;
                      v.AddSubjectGroup(userGroups.Administrators);
                  });

                merge(Grant.GuestId, v =>
                  {
                      v.Role = roles.Guest;
                      v.AddSubjectGroup(userGroups.Guests);
                  });
            }
        }
    }
}

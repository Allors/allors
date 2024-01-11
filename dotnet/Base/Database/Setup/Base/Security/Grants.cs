// <copyright file="Singletons.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Grants
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.Role);
            setup.AddDependency(this.ObjectType, this.M.UserGroup);
        }

        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

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

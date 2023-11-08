// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class Grants
    {
        private UniquelyIdentifiableCache<Grant> cache;

        public UniquelyIdentifiableCache<Grant> Cache => this.cache ??= new UniquelyIdentifiableCache<Grant>(this.Transaction);

        public Grant Creators => this.Cache[Grant.CreatorsId];

        public Grant GuestCreator => this.Cache[Grant.GuestCreatorsId];

        public Grant Administrator => this.Cache[Grant.AdministratorId];

        public Grant Guest => this.Cache[Grant.GuestId];

        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, this.M.Role);
            setup.AddDependency(this.ObjectType, this.M.UserGroup);
        }

        protected override void CoreSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                var merge = this.Cache.Merger().Action();

                var roles = new Roles(this.Transaction);
                var userGroups = new UserGroups(this.Transaction);

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

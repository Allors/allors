// <copyright file="People.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;


    public partial class AutomatedAgents
    {
        private ICache<Guid, AutomatedAgent> cache;

        public ICache<Guid, AutomatedAgent> Cache => this.cache ??= this.Transaction.Caches().AutomatedAgentByUniqueId();

        public AutomatedAgent Guest => this.Cache[AutomatedAgent.GuestId];

        public AutomatedAgent System => this.Cache[AutomatedAgent.SystemId];

        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, this.M.UserGroup);
            setup.AddDependency(this.ObjectType, this.M.SecurityToken);
        }

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Function();

            var guest = merge(AutomatedAgent.GuestId, v => v.UserName = "Guest");
            merge(AutomatedAgent.SystemId, v => v.UserName = "System");

            var userGroups = new UserGroups(this.Transaction);
            userGroups.Guests.AddMember(guest);
        }
    }
}

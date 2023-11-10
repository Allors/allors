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
        private ICache<Guid, Role> cache;

        public Role Administrator => this.Cache[Role.AdministratorId];

        public Role Guest => this.Cache[Role.GuestId];

        public Role GuestCreator => this.Cache[Role.GuestCreatorId];

        public Role Creator => this.Cache[Role.CreatorId];

        public Role Owner => this.Cache[Role.OwnerId];

        public ICache<Guid, Role> Cache => this.cache ??= this.Transaction.Caches().RoleByUniqueId();
        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(Role.AdministratorId, v => v.Name = "Administrator");
            merge(Role.GuestId, v => v.Name = "Guest");
            merge(Role.GuestCreatorId, v => v.Name = "GuestCreator");
            merge(Role.CreatorId, v => v.Name = "Creator");
            merge(Role.OwnerId, v => v.Name = "Owner");
        }
    }
}

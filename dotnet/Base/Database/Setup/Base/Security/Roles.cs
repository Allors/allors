// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    public partial class Roles
    {
        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

            var merge = this.Transaction.Caches().RoleByUniqueId().Merger().Action();

            merge(Role.AdministratorId, v => v.Name = "Administrator");
            merge(Role.GuestId, v => v.Name = "Guest");
            merge(Role.GuestCreatorId, v => v.Name = "GuestCreator");
            merge(Role.CreatorId, v => v.Name = "Creator");
            merge(Role.OwnerId, v => v.Name = "Owner");
        }
    }
}

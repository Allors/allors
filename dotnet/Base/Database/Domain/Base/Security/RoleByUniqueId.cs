// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class RoleByUniqueId
    {
        public Role Administrator => this.cache[Role.AdministratorId];

        public Role Guest => this.cache[Role.GuestId];

        public Role GuestCreator => this.cache[Role.GuestCreatorId];

        public Role Creator => this.cache[Role.CreatorId];

        public Role Owner => this.cache[Role.OwnerId];
    }
}

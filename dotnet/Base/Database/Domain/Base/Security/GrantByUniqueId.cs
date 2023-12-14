// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class GrantByUniqueId
    {
        public Grant Creators => this.cache[Grant.CreatorsId];

        public Grant GuestCreator => this.cache[Grant.GuestCreatorsId];

        public Grant Administrator => this.cache[Grant.AdministratorId];

        public Grant Guest => this.cache[Grant.GuestId];
    }
}

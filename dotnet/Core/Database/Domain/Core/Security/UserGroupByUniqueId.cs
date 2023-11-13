// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class UserGroupByUniqueId : IScoped
    {
        private readonly ICache<Guid, UserGroup> cache;

        public UserGroupByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().UserGroupByUniqueId();
        }

        public UserGroup Administrators => this.cache[UserGroup.AdministratorsId];

        public UserGroup Creators => this.cache[UserGroup.CreatorsId];

        public UserGroup Guests => this.cache[UserGroup.GuestsId];
    }
}

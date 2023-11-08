// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class UserGroups
    {
        private UniquelyIdentifiableCache<UserGroup> cache;

        public UserGroup Administrators => this.Cache[UserGroup.AdministratorsId];

        public UserGroup Creators => this.Cache[UserGroup.CreatorsId];

        public UserGroup Guests => this.Cache[UserGroup.GuestsId];

        private UniquelyIdentifiableCache<UserGroup> Cache => this.cache ??= new UniquelyIdentifiableCache<UserGroup>(this.Transaction);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(UserGroup.AdministratorsId, v => v.Name = "Administrators");
            merge(UserGroup.CreatorsId, v => v.Name = "Creators");
            merge(UserGroup.GuestsId, v => v.Name = "Guests");
        }
    }
}

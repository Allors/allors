// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class UserGroupByUniqueId
    {
        public UserGroup Administrators => this.cache[UserGroup.AdministratorsId];

        public UserGroup Creators => this.cache[UserGroup.CreatorsId];

        public UserGroup Guests => this.cache[UserGroup.GuestsId];
    }
}

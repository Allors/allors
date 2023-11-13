// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    public partial class UserGroups
    {
        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().UserGroupByUniqueId().Merger().Action();

            merge(UserGroup.AdministratorsId, v => v.Name = "Administrators");
            merge(UserGroup.CreatorsId, v => v.Name = "Creators");
            merge(UserGroup.GuestsId, v => v.Name = "Guests");
        }
    }
}

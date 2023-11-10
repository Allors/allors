// <copyright file="TransactionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public static partial class ICachesExtensions
    {
        public static ICache<Guid, Organization> OrganizationByUniqueId(this ICaches @this) => @this.Get<Guid, Organization>(@this.M.Organization, @this.M.Organization.UniqueId);
    }
}

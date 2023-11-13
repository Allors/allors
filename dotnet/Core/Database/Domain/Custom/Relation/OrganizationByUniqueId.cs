// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class OrganizationByUniqueId : IScoped
    {
        private readonly ICache<Guid, Organization> cache;

        public OrganizationByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().OrganizationByUniqueId();
        }

        public Organization this[Guid key]
        {
            get => this.cache[key];
        }
    }
}

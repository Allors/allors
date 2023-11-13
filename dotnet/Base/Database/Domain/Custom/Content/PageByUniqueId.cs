// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PageByUniqueId : IScoped
    {
        private readonly ICache<Guid, Page> cache;

        public PageByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().PageByUniqueId();
        }

        public Page Index => this.cache[Page.IndexId];
    }
}

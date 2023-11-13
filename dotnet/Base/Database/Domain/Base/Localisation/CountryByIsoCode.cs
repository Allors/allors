// <copyright file="Countries.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class CountryByIsoCode : IScoped
    {
        private readonly ICache<string, Country> cache;

        public CountryByIsoCode(ITransaction transaction)
        {
            this.cache = transaction.Caches().CountryByIsoCode();
        }

        public Country this[string key]
        {
            get => this.cache[key];
        }
    }
}

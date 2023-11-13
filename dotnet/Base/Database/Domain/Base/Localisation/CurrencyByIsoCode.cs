// <copyright file="Currencies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class CurrencyByIsoCode : IScoped
    {
        private readonly ICache<string, Currency> cache;

        public CurrencyByIsoCode(ITransaction transaction)
        {
            this.cache = transaction.Caches().CurrencyByIsoCode();
        }

        public Currency this[string key]
        {
            get => this.cache[key];
        }
    }
}

// <copyright file="Locales.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class LocaleByName : IScoped
    {
        private readonly ICache<string, Locale> cache;

        public LocaleByName(ITransaction transaction)
        {
            this.cache = transaction.Caches().LocaleByName();
        }

        public Locale this[string key]
        {
            get => this.cache[key];
        }
    }
}


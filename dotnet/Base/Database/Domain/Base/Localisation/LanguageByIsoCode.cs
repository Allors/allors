// <copyright file="Languages.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class LanguageByIsoCode : IScoped
    {
        private readonly ICache<string, Language> cache;

        public LanguageByIsoCode(ITransaction transaction)
        {
            this.cache = transaction.Caches().LanguageByIsoCode();
        }

        public Language this[string key]
        {
            get => this.cache[key];
        }
    }
}

// <copyright file="Locales.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Linq;

    public partial class Locales
    {
        private ICache<string, Locale> localeByName;

        public ICache<string, Locale> LocaleByName => this.localeByName ??= this.Transaction.Caches().LocaleByName();

        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, this.M.Country);
            setup.AddDependency(this.ObjectType, this.M.Language);
        }

        protected override void CoreSetup(Setup setup)
        {
            var languages = new Languages(this.Transaction);

            var merge = this.LocaleByName.Merger().Action();

            // Create a generic locale (without a region) for every language.
            foreach (var language in languages.Extent().Cast<Language>())
            {
                var name = language.IsoCode.ToLowerInvariant();
                merge(name, v => v.Language = languages.LanguageByIsoCode[name]);
            }
        }
    }
}

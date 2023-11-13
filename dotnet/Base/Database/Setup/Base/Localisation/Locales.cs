// <copyright file="Locales.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Locales
    {
        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, this.M.Country);
            setup.AddDependency(this.ObjectType, this.M.Language);
        }

        protected override void CoreSetup(Setup setup)
        {
            var languages = this.Transaction.Scoped<LanguageByIsoCode>();

            var merge = this.Transaction.Caches().LocaleByName().Merger().Action();

            // Create a generic locale (without a region) for every language.
            foreach (Language language in this.Transaction.Extent<Language>())
            {
                var name = language.IsoCode.ToLowerInvariant();
                merge(name, v => v.Language = languages[name]);
            }
        }
    }
}

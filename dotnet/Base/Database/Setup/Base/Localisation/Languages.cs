// <copyright file="Locales.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Languages
    {
        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

            var locales = this.Transaction.Scoped<LocaleByKey>();

            foreach (Language language in this.Transaction.Extent<Language>())
            {
                var locale = locales[language.Key];
                if (locale != null)
                {
                    locale.Language = language;
                }
            }
        }
    }
}

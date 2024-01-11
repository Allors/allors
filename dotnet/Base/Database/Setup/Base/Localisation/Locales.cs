// <copyright file="Locales.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Locales
    {
        protected override void BasePrepare(Setup setup)
        {
            base.CustomPrepare(setup);

            var merge = this.Transaction.Caches().LocaleByKey().Merger().Action();

            var configuration = setup.Config.Translation.Configuration;

            // Default
            merge(configuration.DefaultCultureInfo.Name, v => { });

            // Additional
            foreach (var cultureInfo in configuration.AdditionalCultureInfos)
            {
                merge(cultureInfo.Name, v => { });
            }
        }
    }
}

// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Singletons
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.Locale);
        }

        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

            var localeByName = this.Transaction.Scoped<LocaleByName>();
            var singleton = this.Transaction.GetSingleton() ?? this.Transaction.Build<Singleton>();
            singleton.DefaultLocale = localeByName["en"];
        }
    }
}

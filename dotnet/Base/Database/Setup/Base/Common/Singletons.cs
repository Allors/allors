// <copyright file="Singletons.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Singletons
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.Locale.Composite);
        }

        protected override void BaseSetup(Setup setup)
        {
            base.BaseSetup(setup);

            var localeByKey = this.Transaction.Scoped<LocaleByKey>();
            var singleton = this.Transaction.GetSingleton() ?? this.Transaction.Build<Singleton>();
            singleton.DefaultLocale = localeByKey["en"];
        }
    }
}

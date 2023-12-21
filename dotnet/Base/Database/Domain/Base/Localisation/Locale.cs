// <copyright file="Locale.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Globalization;

    public partial class Locale
    {
        public bool ExistCultureInfo => this.ExistKey;

        public CultureInfo CultureInfo => this.ExistKey? new CultureInfo(this.Key) : null;

        public void BaseOnInit(ObjectOnInit method)
        {
            if (!this.ExistKey)
            {
                if (this.ExistLanguage)
                {
                    if (this.ExistCountry)
                    {
                        this.Key = this.Language.Key.ToLowerInvariant() + "-" + this.Country.Key.ToUpperInvariant();
                    }
                    else
                    {
                        this.Key = this.Language.Key.ToLowerInvariant();
                    }
                }
            }
        }
    }
}

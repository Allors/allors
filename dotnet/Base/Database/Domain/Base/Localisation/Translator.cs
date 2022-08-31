// <copyright file="Translator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;

    public class Translator<T>
        where T : IObject
    {
        private readonly Locale locale;
        private readonly Locale defaultLocale;

        private readonly Func<T, string> value;
        private readonly Func<T, LocalizedText[]> localizedValues;

        public Translator(Locale locale, Func<T, string> value, Func<T, LocalizedText[]> localizedValues)
        {
            this.locale = locale;
            this.defaultLocale = locale?.Strategy.Transaction.GetSingleton().DefaultLocale;

            this.value = value;
            this.localizedValues = localizedValues;
        }

        public string Translate(T source)
        {
            if (source == null)
            {
                return null;
            }

            if (this.defaultLocale == null || this.locale == null || this.defaultLocale.Equals(this.locale))
            {
                return this.value(source);
            }

            var localizedValue = this.localizedValues(source).FirstOrDefault(v => v.Locale.Equals(this.locale));
            return localizedValue != null ? localizedValue.Text : this.value(source);
        }
    }
}

// <copyright file="LocalizedText.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Meta;

    public class LocalizedTextAccessor
    {
        private readonly IRoleType roleType;

        public LocalizedTextAccessor(IRoleType roleType) => this.roleType = roleType;

        public string Get(IObject @object, Locale locale)
        {
            foreach (var localizedText in @object.Strategy.GetCompositesRole<LocalizedText>(this.roleType))
            {
                if (localizedText?.Locale?.Equals(locale) == true)
                {
                    return localizedText.Text;
                }
            }

            return null;
        }

        public void Set(IObject @object, Locale locale, string text)
        {
            foreach (var existingLocalizedText in @object.Strategy.GetCompositesRole<LocalizedText>(this.roleType))
            {
                if (existingLocalizedText?.Locale?.Equals(locale) == true)
                {
                    existingLocalizedText.Text = text;
                    return;
                }
            }

            var newLocalizedText = @object.Transaction().Build<LocalizedText>(v =>
            {
                v.Locale = locale;
                v.Text = text;
            });
            @object.Strategy.AddCompositesRole(this.roleType, newLocalizedText);
        }
    }
}

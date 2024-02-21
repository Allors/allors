// <copyright file="LocalisedText.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public partial class LocalisedTextAccessor
    {
        private readonly RoleType roleType;

        public LocalisedTextAccessor(RoleType roleType) => this.roleType = roleType;

        public string Get(IObject @object, Locale locale) => @object
            ?.Strategy.GetCompositesRole<LocalisedText>(this.roleType)
            ?.FirstOrDefault(v => locale.Equals(v.Locale))
            ?.Text;

        public void Set(IObject @object, Locale locale, string text)
        {
            foreach (var existingLocalisedText in @object.Strategy.GetCompositesRole<LocalisedText>(this.roleType))
            {
                if (existingLocalisedText?.Locale?.Equals(locale) == true)
                {
                    existingLocalisedText.Text = text;
                    return;
                }
            }

            var newLocalisedText = @object.Strategy.Transaction.Build<LocalisedText>(v =>
            {
                v.Locale = locale;
                v.Text = text;
            });

            @object.Strategy.AddCompositesRole(this.roleType, newLocalisedText);
        }
    }
}

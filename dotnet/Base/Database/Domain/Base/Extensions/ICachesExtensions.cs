// <copyright file="TransactionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public static partial class ICachesExtensions
    {
        public static ICache<Guid, AutomatedAgent> AutomatedAgentByUniqueId(this ICaches @this) => @this.Get<Guid, AutomatedAgent>(@this.M.AutomatedAgent, @this.M.AutomatedAgent.UniqueId);

        public static ICache<string, Country> CountryByIsoCode(this ICaches @this) => @this.Get<string, Country>(@this.M.Country, @this.M.Country.IsoCode);

        public static ICache<string, Currency> CurrencyByIsoCode(this ICaches @this) => @this.Get<string, Currency>(@this.M.Currency, @this.M.Currency.IsoCode);

        public static ICache<string, Language> LanguageByIsoCode(this ICaches @this) => @this.Get<string, Language>(@this.M.Language, @this.M.Language.IsoCode);

        public static ICache<string, Locale> LocaleByName(this ICaches @this) => @this.Get<string, Locale>(@this.M.Locale, @this.M.Locale.Name);

        public static ICache<Guid, Media> MediaByUniqueId(this ICaches @this) => @this.Get<Guid, Media>(@this.M.Media, @this.M.Media.UniqueId);

        public static ICache<Guid, PersistentPreparedExtent> PersistentPreparedExtentByUniqueId(this ICaches @this) => @this.Get<Guid, PersistentPreparedExtent>(@this.M.PersistentPreparedExtent, @this.M.PersistentPreparedExtent.Name);

        public static ICache<Guid, PersistentPreparedSelect> PersistentPreparedSelectByUniqueId(this ICaches @this) => @this.Get<Guid, PersistentPreparedSelect>(@this.M.PersistentPreparedSelect, @this.M.PersistentPreparedSelect.Name);

        public static ICache<Guid, TemplateType> TemplateTypeByUniqueId(this ICaches @this) => @this.Get<Guid, TemplateType>(@this.M.TemplateType, @this.M.TemplateType.UniqueId);
    }
}

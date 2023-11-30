// <copyright file="MetaBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta.Configuration
{
    public partial class MetaBuilder
    {
        private void BuildBase(MetaPopulation meta, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {
            relationTypes.CountryIsoCode.IsKey = true;
            relationTypes.CurrencyIsoCode.IsKey = true;
            relationTypes.EnumerationKey.IsKey = true;
            relationTypes.LanguageIsoCode.IsKey = true;
            relationTypes.LocaleName.IsKey = true;
            relationTypes.UniquelyIdentifiableUniqueId.IsKey = true;
        }
    }
}

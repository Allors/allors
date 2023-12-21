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
            relationTypes.EnumerationKey.IsKey = true;
            relationTypes.UniquelyIdentifiableUniqueId.IsKey = true;
        }
    }
}

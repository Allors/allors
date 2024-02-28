// <copyright file="UniquelyIdentifiableExtension.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public static class UniquelyIdentifiableExtensions
    {
        public static void BaseOnPostBuild(this UniquelyIdentifiable @this, ObjectOnPostBuild method)
        {
            if (!@this.ExistUniqueId)
            {
                @this.Strategy.SetUnitRole(@this.Transaction().Database.Services.Get<IMetaIndex>().UniquelyIdentifiable.UniqueId, Guid.NewGuid());
            }
        }
    }
}

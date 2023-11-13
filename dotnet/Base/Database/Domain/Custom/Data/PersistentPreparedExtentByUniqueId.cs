// <copyright file="PersistentPreparedExtents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PersistentPreparedExtentByUniqueId : IScoped
    {
        private readonly ICache<Guid, PersistentPreparedExtent> cache;

        public PersistentPreparedExtentByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().PersistentPreparedExtentByUniqueId();
        }

        public PersistentPreparedExtent this[Guid key]
        {
            get => this.cache[key];
        }

        public PersistentPreparedExtent ByName => this.cache[PersistentPreparedExtent.ByNameId];
    }
}


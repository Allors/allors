// <copyright file="PersistentPreparedExtents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PersistentPreparedExtents
    {
        private ICache<Guid, PersistentPreparedExtent> cache;

        public ICache<Guid, PersistentPreparedExtent> Cache => this.cache ??= this.Transaction.Caches().PersistentPreparedExtentByUniqueId();
    }
}

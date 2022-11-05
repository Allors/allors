// <copyright file="ICaches.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Concurrent;

    public class VersionedIdByStrategy : IVersionedIdByStrategy
    {
        private readonly ConcurrentDictionary<IStrategy, VersionedObjectId> versionedByStrategy;

        public VersionedIdByStrategy() => this.versionedByStrategy = new ConcurrentDictionary<IStrategy, VersionedObjectId>();

        public VersionedObjectId Get(IStrategy strategy)
        {
            if (!this.versionedByStrategy.TryGetValue(strategy, out var versionedId) || versionedId.ObjectVersion != strategy.ObjectVersion)
            {
                versionedId = new VersionedObjectId(strategy.ObjectId, strategy.ObjectVersion);
                this.versionedByStrategy[strategy] = versionedId;
            }

            return versionedId;
        }
    }
}

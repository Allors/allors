// <copyright file="Cache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class Cache<TKey, TObject> : ICache<TKey, TObject>
        where TObject : class, IObject
    {
        private IDictionary<TKey, long> cache;

        public Cache(ITransaction transaction, RoleType roleType)
        {
            if (!roleType.ObjectType.IsUnit)
            {
                throw new ArgumentException("ObjectType of RoleType should be a Unit");
            }

            this.Transaction = transaction;
            this.RoleType = roleType;
        }

        public ITransaction Transaction { get; }

        public RoleType RoleType { get; }

        public TObject this[TKey key]
        {
            get
            {
                Type type = typeof(TObject);
                var key1 = $"{type}.{this.RoleType}";

                var caches = this.Transaction.Database.Services.Get<IDatabaseCaches>();
                var cache1 = caches.Get<TKey>(key1);
                if (cache1 == null)
                {
                    cache1 = new ConcurrentDictionary<TKey, long>();
                    caches.Set(key1, cache1);
                }

                this.cache ??= cache1;

                if (!this.cache.TryGetValue(key, out var objectId))
                {
                    var extent = this.Transaction.Extent<TObject>();
                    extent.Predicate.AddEquals(this.RoleType, key);

                    var @object = extent.FirstOrDefault();
                    if (@object != null)
                    {
                        objectId = @object.Id;
                        if (!@object.Strategy.IsNewInTransaction)
                        {
                            this.cache[key] = @object.Id;
                        }
                    }
                }

                return (TObject)this.Transaction.Instantiate(objectId);
            }
        }

        public ICacheMerger<TKey, TObject> Merger(Action<TObject>? defaults = null) => new CacheMerger<TKey, TObject>(this, defaults);
    }
}

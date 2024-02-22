// <copyright file="IDatabaseCaches.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using Meta;

    public class Caches : ICaches
    {
        private readonly IDictionary<Composite, IDictionary<RoleType, object>> cacheByRoleTypeByObjectType;
      
        public Caches(ITransaction transaction, MetaIndex m)
        {
            this.Transaction = transaction;
            this.M = m;
            this.cacheByRoleTypeByObjectType = new Dictionary<Composite, IDictionary<RoleType, object>>();
        }

        public ITransaction Transaction { get; }

        public MetaIndex M { get; }
        
        public ICache<TKey, TObject> Get<TKey, TObject>(Composite objectType, RoleType roleType) where TObject : class, IObject
        {
            if (!this.cacheByRoleTypeByObjectType.TryGetValue(objectType, out var cacheByRoleType))
            {
                cacheByRoleType = new Dictionary<RoleType, object>();
                this.cacheByRoleTypeByObjectType.Add(objectType, cacheByRoleType);
            }

            if (!cacheByRoleType.TryGetValue(roleType, out var cache))
            {
                cache = new Cache<TKey, TObject>(this.Transaction, roleType);
                cacheByRoleType.Add(roleType, cache);
            }

            return (ICache<TKey, TObject>)cache;
        }
    }
}

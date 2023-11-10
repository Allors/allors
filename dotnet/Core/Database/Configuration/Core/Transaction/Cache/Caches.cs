// <copyright file="IDatabaseCaches.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using Meta;

    public class Caches : ICaches
    {
        private readonly IDictionary<IComposite, IDictionary<IRoleType, object>> cacheByRoleTypeByObjectType;
      
        public Caches(ITransaction transaction, M m)
        {
            this.Transaction = transaction;
            this.M = m;
        }

        public ITransaction Transaction { get; }

        public M M { get; }
        
        public ICache<TKey, TObject> Get<TKey, TObject>(IComposite objectType, IRoleType roleType) where TObject : class, IObject
        {
            if (!this.cacheByRoleTypeByObjectType.TryGetValue(objectType, out var cacheByRoleType))
            {
                cacheByRoleType = new Dictionary<IRoleType, object>();
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

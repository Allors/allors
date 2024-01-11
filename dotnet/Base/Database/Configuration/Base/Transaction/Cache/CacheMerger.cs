// <copyright file="Cache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using Meta;

    public class CacheMerger<TKey, TObject> : ICacheMerger<TKey, TObject>
           where TObject : class, IObject
    {
        private readonly Cache<TKey, TObject> cache;
        private readonly Action<TObject>? defaults;
        private readonly ITransaction transaction;
        private readonly IClass @class;
        private readonly IRoleType roleType;

        internal CacheMerger(Cache<TKey, TObject> cache, Action<TObject>? defaults)
        {
            this.cache = cache;
            this.defaults = defaults;
            this.transaction = cache.Transaction;
            this.@class = (IClass)this.transaction.Database.ObjectFactory.GetObjectType(typeof(TObject));
            this.roleType = this.cache.RoleType;
        }

        public Func<TKey, Action<TObject>, TObject> Function() =>
            (id, action) => this.Merge(id, action);

        public Action<TKey, Action<TObject>> Action() =>
            (id, action) => this.Merge(id, action);

        public TObject Merge(TKey id, Action<TObject> action)
        {
            var @object = this.cache[id] ?? (TObject)this.transaction.Build(this.@class);
            @object.Strategy.SetUnitRole(this.roleType, id);

            this.defaults?.Invoke(@object);
            action(@object);

            return @object;
        }
    }
}

// <copyright file="IDatabaseCaches.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public interface ICache<TKey, TObject> where TObject : class, IObject
    {
        public TObject this[TKey key]
        {
            get;
        }

        ICacheMerger<TKey, TObject> Merger(Action<TObject>? defaults = null);
    }
}

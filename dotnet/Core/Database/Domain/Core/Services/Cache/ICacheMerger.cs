// <copyright file="Cache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public interface ICacheMerger<TKey, TObject> 
        where TObject : class, IObject
    {

        Func<TKey, Action<TObject>, TObject> Function() =>
            (id, action) => this.Merge(id, action);

        Action<TKey, Action<TObject>> Action() =>
            (id, action) => this.Merge(id, action);

        TObject Merge(TKey id, Action<TObject> action);
    }
}

// <copyright file="IDatabaseCaches.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Meta;

    public interface ICaches
    {
        M M { get; }

        ICache<TKey, TObject> Get<TKey, TObject>(IComposite objectType, IRoleType roleType) where TObject : class, IObject;
    }
}

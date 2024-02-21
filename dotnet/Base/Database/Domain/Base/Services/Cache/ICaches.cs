﻿// <copyright file="IDatabaseCaches.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Meta;

    public interface ICaches
    {
        M M { get; }

        ICache<TKey, TObject> Get<TKey, TObject>(IComposite objectType, RoleType roleType) where TObject : class, IObject;
    }
}

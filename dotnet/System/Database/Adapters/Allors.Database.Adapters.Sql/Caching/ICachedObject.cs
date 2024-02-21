// <copyright file="ICachedObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching;

using Allors.Database.Meta;

public interface ICachedObject
{
    long Version { get; }

    bool Contains(RoleType roleType);

    bool TryGetValue(RoleType roleType, out object value);

    void SetValue(RoleType roleType, object value);
}

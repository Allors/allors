// <copyright file="DefaultCachedObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching;

using System.Collections.Concurrent;
using Allors.Database.Meta;

public sealed class DefaultCachedObject : ICachedObject
{
    private readonly ConcurrentDictionary<RoleType, object> roleByRoleType;

    internal DefaultCachedObject(long version)
    {
        this.Version = version;
        this.roleByRoleType = new ConcurrentDictionary<RoleType, object>();
    }

    public long Version { get; }

    public bool Contains(RoleType roleType) => this.roleByRoleType.ContainsKey(roleType);

    public bool TryGetValue(RoleType roleType, out object value) => this.roleByRoleType.TryGetValue(roleType, out value);

    public void SetValue(RoleType roleType, object value) => this.roleByRoleType[roleType] = value;
}

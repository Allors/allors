// <copyright file="FromJson.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json;

using System.Collections.Generic;
using System.Linq;
using Allors.Database.Data;

public class IntersectsResolver : IResolver
{
    private readonly Intersects intersects;
    private readonly long[] objectIds;

    public IntersectsResolver(Intersects intersects, long[] objectIds)
    {
        this.intersects = intersects;
        this.objectIds = objectIds;
    }

    public void Prepare(HashSet<long> objectIds) => objectIds.UnionWith(this.objectIds);

    public void Resolve(Dictionary<long, IObject> objectById) =>
        this.intersects.Objects = this.objectIds.Select(v => objectById[v]).ToArray();
}

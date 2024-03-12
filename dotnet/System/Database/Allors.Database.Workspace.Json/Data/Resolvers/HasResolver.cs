// <copyright file="FromJson.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json;

using System.Collections.Generic;
using Allors.Database.Data;

public class HasResolver : IResolver
{
    private readonly Has has;
    private readonly long objectId;

    public HasResolver(Has has, long objectId)
    {
        this.has = has;
        this.objectId = objectId;
    }

    public void Prepare(HashSet<long> objectIds) => objectIds.Add(this.objectId);

    public void Resolve(Dictionary<long, IObject> objectById) => this.has.Object = objectById[this.objectId];
}

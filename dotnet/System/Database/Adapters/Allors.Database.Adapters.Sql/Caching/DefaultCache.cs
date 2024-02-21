// <copyright file="DefaultCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching;

using System.Collections.Concurrent;
using System.Collections.Generic;
using Allors.Database.Meta;

/// <summary>
///     The Cache holds a CachedObject and/or IObjectType by ObjectId.
/// </summary>
public sealed class DefaultCache : ICache
{
    private readonly ConcurrentDictionary<long, DefaultCachedObject> cachedObjectByObjectId;
    private readonly HashSet<Class> excludedClasses;
    private readonly ConcurrentDictionary<long, Class> objectTypeByObjectId;

    internal DefaultCache(Class[] excludedClasses)
    {
        this.cachedObjectByObjectId = new ConcurrentDictionary<long, DefaultCachedObject>();
        this.objectTypeByObjectId = new ConcurrentDictionary<long, Class>();

        if (excludedClasses != null)
        {
            this.excludedClasses = new HashSet<Class>();
            foreach (var transientObjectType in excludedClasses)
            {
                foreach (var transientClass in transientObjectType.Classes)
                {
                    this.excludedClasses.Add(transientClass);
                }
            }

            if (this.excludedClasses.Count == 0)
            {
                this.excludedClasses = null;
            }
        }
    }

    /// <summary>
    ///     Invalidates the Cache.
    /// </summary>
    public void Invalidate()
    {
        this.cachedObjectByObjectId.Clear();
        this.objectTypeByObjectId.Clear();
    }

    public ICachedObject GetOrCreateCachedObject(Class concreteClass, long objectId, long version)
    {
        if (this.excludedClasses != null && this.excludedClasses.Contains(concreteClass))
        {
            return new DefaultCachedObject(version);
        }

        if (this.cachedObjectByObjectId.TryGetValue(objectId, out var cachedObject))
        {
            if (!cachedObject.Version.Equals(version))
            {
                cachedObject = new DefaultCachedObject(version);
                this.cachedObjectByObjectId[objectId] = cachedObject;
            }
        }
        else
        {
            cachedObject = new DefaultCachedObject(version);
            this.cachedObjectByObjectId[objectId] = cachedObject;
        }

        return cachedObject;
    }

    public Class GetObjectType(long objectId)
    {
        this.objectTypeByObjectId.TryGetValue(objectId, out var objectType);
        return objectType;
    }

    public void SetObjectType(long objectId, Class objectType) => this.objectTypeByObjectId[objectId] = objectType;

    public void OnCommit(IList<long> accessedObjectIds, IList<long> changedObjectIds)
    {
        if (changedObjectIds.Count > 0)
        {
            foreach (var changedObjectId in changedObjectIds)
            {
                this.cachedObjectByObjectId.TryRemove(changedObjectId, out var removedObject);
            }
        }
    }

    public void OnRollback(List<long> accessedObjectIds)
    {
    }
}

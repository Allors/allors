// <copyright file="Subscriber.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Default type.</summary>
//------------------------------------------------------------------------------------------------

namespace Allors.Database;

using System;
using Allors.Database.Configuration;
using Allors.Database.Services;
using Meta;

public class DefaultDomainDatabaseServices : IDomainDatabaseServices
{
    private IMetaCache metaCache;

    public virtual void OnInit(IDatabase database)
    {
        this.M = (M)database.MetaPopulation;
        this.metaCache = new MetaCache(database);
    }

    public ITransactionServices CreateTransactionServices() => new DefaultDomainTransactionServices();

    public M M { get; private set; }

    public T Get<T>() =>
        typeof(T) switch
        {
            // System
            { } type when type == typeof(IMetaCache) => (T)this.metaCache,
            _ => throw new NotSupportedException($"Service {typeof(T)} not supported"),
        };

    public void Dispose() { }
}

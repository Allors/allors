// <copyright file="PreparedExtents.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Data;
    using Domain;
    using Meta;
    using Services;
    using PersistentPreparedExtent = Domain.PersistentPreparedExtent;

    public class PreparedExtents : IPreparedExtents
    {
        private readonly ConcurrentDictionary<Guid, IExtent> extentById;

        public PreparedExtents(IDatabase database)
        {
            this.Database = database;
            this.extentById = new ConcurrentDictionary<Guid, IExtent>();
        }

        public IDatabase Database { get; }
        
        public static Guid OrganizationByName => new Guid("5D8D1C36-4ABD-4969-BCDC-4B6FA2454D65");
        
        public IExtent Get(Guid id)
        {
            if (id == OrganizationByName)
            {
                var m = this.Database.Services.Get<IMetaIndex>();
                return new Extent(m.Organization) { Predicate = new Equals(m.Organization.Name) { Parameter = "name" } };
            }

            if (!this.extentById.TryGetValue(id, out var extent))
            {
                var transaction = this.Database.CreateTransaction();
                try
                {
                    var m = transaction.Database.Services.Get<IMetaIndex>();

                    var filter = new Extent(m.PersistentPreparedExtent)
                    {
                        Predicate = new Equals(m.PersistentPreparedExtent.UniqueId) { Value = id },
                    };

                    var preparedExtent = (PersistentPreparedExtent)filter.Build(transaction).First();
                    if (preparedExtent != null)
                    {
                        extent = preparedExtent.Extent;
                        this.extentById[id] = extent;
                    }
                }
                finally
                {
                    if (this.Database.IsShared)
                    {
                        transaction.Dispose();
                    }
                }
            }

            return extent;
        }
    }
}

// <copyright file="IDatabaseCaches.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Concurrent;
    using Allors.Database.Meta;

    public class ClassById : IClassById
    {
        private readonly ConcurrentDictionary<long, Class> classById;

        public ClassById() => this.classById = new ConcurrentDictionary<long, Class>();

        public Class Get(long id)
        {
            this.classById.TryGetValue(id, out var @class);
            return @class;
        }

        public void Set(long id, Class @class) => this.classById[id] = @class;
    }
}

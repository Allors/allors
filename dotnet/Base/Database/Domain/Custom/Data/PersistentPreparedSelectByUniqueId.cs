// <copyright file="PersistentPreparedSelects.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PersistentPreparedSelectByUniqueId : IScoped
    {
        private readonly ICache<Guid, PersistentPreparedSelect> cache;

        public PersistentPreparedSelectByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().PersistentPreparedSelectByUniqueId();
        }

        public PersistentPreparedSelect this[Guid key]
        {
            get => this.cache[key];
        }

        public PersistentPreparedSelect SelectPeople => this.cache[PersistentPreparedSelect.SelectPeopleId];
    }
}


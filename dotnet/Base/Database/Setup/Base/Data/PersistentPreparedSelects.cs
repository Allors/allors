// <copyright file="PersistentPreparedSelects.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class PersistentPreparedSelects
    {
        private ICache<Guid, PersistentPreparedSelect> cache;

        public ICache<Guid, PersistentPreparedSelect> Cache => this.cache ??= this.Transaction.Caches().PersistentPreparedSelectByUniqueId();
    }
}

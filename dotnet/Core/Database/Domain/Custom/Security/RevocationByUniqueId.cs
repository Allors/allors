// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class RevocationByUniqueId : IScoped
    {
        private readonly ICache<Guid, Revocation> cache;

        public RevocationByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().RevocationByUniqueId();
        }

        public Revocation ToggleRevocation => this.cache[Revocation.ToggleRevocationId];
    }
}

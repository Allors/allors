// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class Revocations
    {
        private ICache<Guid, Revocation> cache;

        public ICache<Guid, Revocation> Cache => this.cache ??= this.Transaction.Caches().RevocationByUniqueId();
    }
}

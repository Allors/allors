// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class SecurityTokenByUniqueId : IScoped
    {
        private readonly ICache<Guid, SecurityToken> cache;

        public SecurityTokenByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().SecurityTokenByUniqueId();
        }

        public SecurityToken InitialSecurityToken => this.cache[SecurityToken.InitialSecurityTokenId];

        public SecurityToken DefaultSecurityToken => this.cache[SecurityToken.DefaultSecurityTokenId];

        public SecurityToken AdministratorSecurityToken => this.cache[SecurityToken.AdministratorSecurityTokenId];
    }
}

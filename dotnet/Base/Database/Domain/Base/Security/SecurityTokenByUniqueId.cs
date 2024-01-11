// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class SecurityTokenByUniqueId
    {
        public SecurityToken InitialSecurityToken => this.cache[SecurityToken.InitialSecurityTokenId];

        public SecurityToken DefaultSecurityToken => this.cache[SecurityToken.DefaultSecurityTokenId];

        public SecurityToken AdministratorSecurityToken => this.cache[SecurityToken.AdministratorSecurityTokenId];
    }
}

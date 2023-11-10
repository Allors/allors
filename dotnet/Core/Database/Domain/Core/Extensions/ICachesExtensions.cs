// <copyright file="TransactionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public static partial class ICachesExtensions
    {
        public static ICache<Guid, Grant> GrantByUniqueId(this ICaches @this) => @this.Get<Guid, Grant>(@this.M.Grant, @this.M.Grant.UniqueId);

        public static ICache<Guid, Revocation> RevocationByUniqueId(this ICaches @this) => @this.Get<Guid, Revocation>(@this.M.Revocation, @this.M.Revocation.UniqueId);

        public static ICache<Guid, Role> RoleByUniqueId(this ICaches @this) => @this.Get<Guid, Role>(@this.M.Role, @this.M.Role.UniqueId);

        public static ICache<Guid, SecurityToken> SecurityTokenByUniqueId(this ICaches @this) => @this.Get<Guid, SecurityToken>(@this.M.SecurityToken, @this.M.SecurityToken.UniqueId);

        public static ICache<Guid, UserGroup> UserGroupByUniqueId(this ICaches @this) => @this.Get<Guid, UserGroup>(@this.M.UserGroup, @this.M.UserGroup.UniqueId);
    }
}

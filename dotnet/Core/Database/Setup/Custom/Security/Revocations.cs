// <copyright file="Roles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    public partial class Revocations
    {
        protected override void CustomSecure(Security security)
        {
            base.CustomSecure(security);

            var merge = this.Transaction.Caches().RevocationByUniqueId().Merger().Action();

            merge(Revocation.ToggleRevocationId, _ => { });
        }
    }
}

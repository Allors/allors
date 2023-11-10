// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Database.Domain
{
    using System;
    
    public partial class Organisations
    {
        private ICache<Guid, Organisation> cache;

        public ICache<Guid, Organisation> Cache => this.cache ??= this.Transaction.Caches().OrganisationByUniqueId();

        protected override void CustomPrepare(Security security) => security.AddDependency(this.ObjectType, this.M.Revocation);

        protected override void CustomSecure(Security security)
        {
            var revocations = new Revocations(this.Transaction);
            var permissions = new Permissions(this.Transaction);

            revocations.ToggleRevocation.DeniedPermissions = new[]
            {
                permissions.Get(this.Meta, this.Meta.Name, Operations.Write),
                permissions.Get(this.Meta, this.Meta.Owner, Operations.Write),
                permissions.Get(this.Meta, this.Meta.Employees, Operations.Write),
            };
        }
    }
}

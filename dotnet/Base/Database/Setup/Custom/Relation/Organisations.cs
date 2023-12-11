// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Database.Domain
{
    public partial class Organisations
    {
        protected override void CustomPrepare(Security security)
        {
            base.CustomPrepare(security);

            security.AddDependency(this.ObjectType, this.M.Revocation);
        }

        protected override void CustomSecure(Security security)
        {
            base.CustomSecure(security);

            var revocations = this.Transaction.Scoped<RevocationByUniqueId>();
            var permissions = this.Transaction.Scoped<PermissionByMeta>();

            revocations.ToggleRevocation.DeniedPermissions = new[]
            {
                permissions.Get(this.Meta, this.Meta.Name, Operations.Write),
                permissions.Get(this.Meta, this.Meta.Owner, Operations.Write),
                permissions.Get(this.Meta, this.Meta.Employees, Operations.Write),
            };
        }
    }
}

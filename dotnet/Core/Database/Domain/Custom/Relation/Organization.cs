// <copyright file="Organization.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Database.Domain
{
    using System;

    public partial class Organization
    {
        public void CustomOnPostDerive(ObjectOnPostDerive _) => this.PostDeriveTrigger = true;

        public void CustomToggleCanWrite(OrganizationToggleCanWrite method)
        {
            if (this.ExistRevocations)
            {
                this.RemoveRevocations();
            }
            else
            {
                var cache = this.Transaction().Scoped<RevocationByUniqueId>();
                var toggleRevocation = cache.ToggleRevocation;
                this.AddRevocation(toggleRevocation);
            }
        }

        public void CustomJustDoIt(OrganizationJustDoIt _) => this.JustDidIt = true;

        public override string ToString() => this.Name;
    }
}

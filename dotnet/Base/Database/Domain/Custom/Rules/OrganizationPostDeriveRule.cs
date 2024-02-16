// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;
    using Allors.Database.Meta;

    public class OrganizationPostDeriveRule : Rule
    {
        public OrganizationPostDeriveRule(M m) 
            : base(m, new Guid("755E60CF-1D5E-4D24-8FDE-396FF7C3030B")) =>
            this.Patterns =
            [
                new RolePattern<MetaOrganization>(m.Organization, v => v.PostDeriveTrigger),
            ];

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var organization in matches.Cast<Organization>())
            {
                organization.PostDeriveTriggered = organization.PostDeriveTrigger;
            }
        }
    }
}

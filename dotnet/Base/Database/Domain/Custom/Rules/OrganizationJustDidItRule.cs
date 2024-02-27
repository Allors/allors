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

    public class OrganizationJustDidItRule : Rule<Organization>
    {
        public OrganizationJustDidItRule(IMetaIndex m)
            : base(m, new Guid("69C87CD7-52DE-45ED-8709-898A3A701A71")) =>
            this.Patterns =
            [
                new RolePattern<Organization, Organization>(m.Organization.JustDidIt),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Organization> matches)
        {
            foreach (var organization in matches)
            {
                organization.JustDidItDerived = organization.JustDidIt;
            }
        }
    }
}

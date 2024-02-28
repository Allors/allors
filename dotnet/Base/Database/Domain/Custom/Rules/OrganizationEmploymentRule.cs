// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Derivations.Rules;

    public class OrganizationEmploymentRule : Rule<Organization, OrganizationIndex>
    {
        public OrganizationEmploymentRule(IMetaIndex m) : base(m, m.Organization, new Guid("4B144553-5EED-4B52-BFB3-FACE609C6341")) =>
            this.Patterns =
            [
                this.Builder.Pattern<Employment>(m.Employment.FromDate, v=>v.Employer),
                this.Builder.Pattern<Employment>(m.Employment.ThroughDate, v=>v.Employer),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Organization> matches)
        {
            var transaction = cycle.Transaction;

            foreach (var @this in matches)
            {
                var now = @this.Transaction().Now();

                var employments = @this.EmploymentsWhereEmployer.ToArray();

                @this.ActiveEmployments = employments
                    .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                    .ToArray();

                @this.InactiveEmployments = employments
                    .Except(@this.ActiveEmployments)
                    .ToArray();

                @this.ActiveEmployees = @this.ActiveEmployments
                    .Select(v => v.Employee)
                    .ToArray();
            }
        }
    }
}

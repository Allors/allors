// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Database.Derivations;
    using Derivations.Rules;

    public class PersonFullNameRule : Rule<Person, PersonIndex>
    {
        public PersonFullNameRule(IMetaIndex m) : base(m, m.Person, new Guid("CDE0A670-4490-41ED-944E-7DFDF41B672B")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.FirstName),
                this.Builder.Pattern(v=>v.LastName),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Person> matches)
        {
            foreach (var person in matches)
            {
                person.DomainFullName = $"{person.FirstName} {person.LastName}";
            }
        }
    }
}

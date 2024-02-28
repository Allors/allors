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

    public class PersonGreetingRule : Rule<Person, PersonIndex>
    {
        public PersonGreetingRule(IMetaIndex m) : base(m, m.Person, new Guid("5FFD5696-E735-4D05-8405-3A444B6F591E")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.DomainFullName),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Person> matches)
        {
            foreach (var person in matches)
            {
                person.DomainGreeting = $"Hello {person.DomainFullName}!";
            }
        }
    }
}

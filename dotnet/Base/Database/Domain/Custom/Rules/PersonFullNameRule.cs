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
    using Meta;
    using Derivations.Rules;

    public class PersonFullNameRule : Rule
    {
        public PersonFullNameRule(IMetaIndex m) : base(m, new Guid("CDE0A670-4490-41ED-944E-7DFDF41B672B")) =>
            this.Patterns =
            [
                new RolePattern(m.Person.FirstName, m.Person),
                new RolePattern(m.Person.LastName, m.Person),
            ];

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var person in matches.Cast<Person>())
            {
                person.DomainFullName = $"{person.FirstName} {person.LastName}";
            }
        }
    }
}

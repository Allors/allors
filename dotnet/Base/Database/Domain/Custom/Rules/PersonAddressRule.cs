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

    public class PersonAddressRule : Rule<Person>
    {
        public PersonAddressRule(IMetaIndex m) : base(m, new Guid("E6F95E43-838D-47DF-AC8A-F1B9CB89995F")) =>
            this.Patterns =
            [
                new RolePattern<Person, Person>(m.Person.MainAddress),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Person> matches)
        {
            foreach (var @this in matches)
            {
                @this.Address = @this.MainAddress;
            }
        }
    }
}

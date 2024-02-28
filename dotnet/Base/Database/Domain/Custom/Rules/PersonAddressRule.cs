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

    public class PersonAddressRule : Rule<Person, PersonIndex>
    {
        public PersonAddressRule(IMetaIndex m) : base(m, m.Person, new Guid("E6F95E43-838D-47DF-AC8A-F1B9CB89995F")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.MainAddress),
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

// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;

    public class PersonOwningRule : Rule<Person, PersonIndex>
    {
        public PersonOwningRule(IMetaIndex m) : base(m, m.Person, new Guid("31564037-C654-45AA-BC2B-69735A93F227")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.OrganizationsWhereOwner),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Person> matches)
        {
            foreach (var person in matches)
            {
                person.Owning = person.ExistOrganizationsWhereOwner;
            }
        }
    }
}

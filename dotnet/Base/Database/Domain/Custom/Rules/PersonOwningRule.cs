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

    public class PersonOwningRule : Rule
    {
        public PersonOwningRule(M m) : base(m, new Guid("31564037-C654-45AA-BC2B-69735A93F227")) =>
            this.Patterns = new Pattern[]
            {
                m.Person.AssociationPattern(v => v.OrganizationsWhereOwner),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var person in matches.Cast<Person>())
            {
                person.Owning = person.ExistOrganizationsWhereOwner;
            }
        }
    }
}

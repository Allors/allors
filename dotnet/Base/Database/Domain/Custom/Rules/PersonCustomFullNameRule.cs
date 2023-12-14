// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Data;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;
    using Allors.Database.Meta;

    public class PersonCustomFullNameRule : Rule
    {
        public PersonCustomFullNameRule(M m) : base(m, new Guid("7059C274-1EB7-42CD-89EC-44A1512E0335")) =>
            this.Patterns = new IRolePattern[]
            {
                new CustomRolePattern(m.Person.FirstName),
                new CustomRolePattern(m.Person.LastName),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            foreach (var person in matches.Cast<Person>())
            {
                person.CustomFullName = $"{person.FirstName} {person.LastName}";
            }
        }

        private class CustomRolePattern : IRolePattern
        {
            public CustomRolePattern(IRoleType roleType) => this.RoleType = roleType;

            public IEnumerable<Node> Tree => null;

            public IComposite OfType => null;

            public IComposite ObjectType => null;

            public IRoleType RoleType { get; }
        }
    }
}

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

    public class DataRule : Rule<Data>
    {
        public DataRule(IMetaIndex m) : base(m, new Guid("B3CADA5C-B844-40BF-82B9-CF4EC41AF198")) =>
            this.Patterns =
            [
                new RolePattern<Data, Data>(m.Data.AutocompleteAssignedFilter),
                new RolePattern<Data, Data>(m.Data.AutocompleteAssignedOptions),
                new RolePattern<Data, Data>(m.Data.SelectAssigned),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Data> matches)
        {
            foreach (var @this in matches)
            {
                @this.AutocompleteDerivedFilter = @this.AutocompleteAssignedFilter;
                @this.AutocompleteDerivedOptions = @this.AutocompleteAssignedOptions;
                @this.SelectDerived = @this.SelectAssigned;
            }
        }
    }
}

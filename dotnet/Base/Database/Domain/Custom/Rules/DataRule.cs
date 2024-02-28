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

    public class DataRule : Rule<Data, DataIndex>
    {
        public DataRule(IMetaIndex m) : base(m, m.Data, new Guid("B3CADA5C-B844-40BF-82B9-CF4EC41AF198")) =>
            this.Patterns =
            [
                this.Builder.Pattern(v=>v.AutocompleteAssignedFilter),
                this.Builder.Pattern(v=>v.AutocompleteAssignedOptions),
                this.Builder.Pattern(v=>v.SelectAssigned),
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

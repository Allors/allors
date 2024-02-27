
// <copyright file="ValidationBase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Derivations;

    public abstract class Rule : IRule
    {
        protected Rule(IMetaIndex m, Guid id)
        {
            this.M = m;
            this.Id = id;
        }

        public IMetaIndex M { get; }

        public Guid Id { get; }

        public IEnumerable<IPattern> Patterns { get; protected set; }

        public abstract void Derive(ICycle cycle, IEnumerable<IObject> matches);
    }

    public abstract class Rule<T> : Rule, IRule<T>
            where T : class, IObject
    {
        protected Rule(IMetaIndex m, Guid id)
            : base(m, id)
        {
        }

        public abstract void Derive(ICycle cycle, IEnumerable<T> matches);

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            this.Derive(cycle, matches.Cast<T>());
        }
    }
}

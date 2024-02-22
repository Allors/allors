﻿// <copyright file="DomainTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Tests
{
    using System;
    using Allors.Database.Configuration.Derivations.Default;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Allors.Database.Meta.Configuration;

    public class Fixture : IDisposable
    {
        private static readonly MetaBuilder MetaBuilder = new MetaBuilder();

        public Fixture()
        {
            this.MetaPopulation = MetaBuilder.Build();
            this.M = new MetaIndex(this.MetaPopulation);
            var rules = Rules.Create(this.M);
            this.Engine = new Engine(rules);
        }

        public MetaPopulation MetaPopulation { get; set; }

        public IMetaIndex M { get; set; }

        public Engine Engine { get; set; }

        public void Dispose() => this.M = null;
    }
}

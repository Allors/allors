// <copyright file="RecordsByClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>


using Allors.Database.Meta.Configuration;

namespace Allors.Workspace.WinForms.ViewModels.Tests
{
    using System;
    using Allors.Database.Configuration.Derivations.Default;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    
    public class Fixture : IDisposable
    {
        private static readonly MetaBuilder MetaBuilder = new MetaBuilder();

        public Fixture()
        {
            var metaPopulation = MetaBuilder.Build();
            this.M = new MetaIndex(metaPopulation);
            var rules = Rules.Create(this.M);
            this.Engine = new Engine(rules);
        }

        public IMetaIndex M { get; private set; }

        public Engine Engine { get; }

        public void Dispose() => this.M = null;
    }
}

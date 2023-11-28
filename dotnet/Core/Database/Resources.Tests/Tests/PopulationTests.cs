// <copyright file="FilterTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Resources.Tests
{
    using Database.Meta.Configuration;
    using Xunit;

    public class PopulationTests
    {
        [Fact]
        public void Build()
        {
            var metaBuilder = new MetaBuilder();
            var metaPopulation = metaBuilder.Build();
            var population = new Population(metaPopulation);

            Assert.NotNull(population.XmlDocument);
        }

    }
}

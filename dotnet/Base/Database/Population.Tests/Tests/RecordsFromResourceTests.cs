// <copyright file="FilterTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Resources.Tests
{
    using Database.Meta.Configuration;
    using Database.Population;
    using Xunit;

    public class RecordsFromResourceTests
    {
        [Fact]
        public void Build()
        {
            var metaBuilder = new MetaBuilder();
            var metaPopulation = metaBuilder.Build();
            var recordsFromResource = new RecordsFromResource(metaPopulation);
            var recordsByClass = recordsFromResource.RecordsByClass;

            Assert.NotNull(recordsByClass);
        }
    }
}

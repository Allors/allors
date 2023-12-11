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
    using Population;
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

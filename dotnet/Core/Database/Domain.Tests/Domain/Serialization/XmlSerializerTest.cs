// <copyright file="ToJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class XmlSerializerTest : DomainTest, IClassFixture<Fixture>
    {
        public XmlSerializerTest(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Serialize()
        {
            var xmlSerializer = new XmlSerializer();

            var people = this.Transaction.Extent<Person>();

            var document = xmlSerializer.Serialize(people);

            Assert.NotNull(document);
        }
    }
}

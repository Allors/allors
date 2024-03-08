// <copyright file="DerivationLogTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Xunit;

    public class ValidationTests : DomainTest, IClassFixture<Fixture>
    {
        public ValidationTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void AssertIsUniqueTest()
        {
            var valiData = this.Transaction.Build<ValiData>();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            valiData.RequiredPerson = this.Transaction.Filter<Person>().First();

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}

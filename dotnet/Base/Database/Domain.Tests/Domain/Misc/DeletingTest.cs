// <copyright file="BuilderTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class DeletingTest : DomainTest, IClassFixture<Fixture>
    {
        public DeletingTest(Fixture fixture) : base(fixture) { }

        [Fact]
        public void IsDeleting()
        {
            var cascader = this.Transaction.Build<Cascader>(v => v.Cascaded = this.Transaction.Build<Cascaded>());
            cascader.Delete();
        }
    }
}

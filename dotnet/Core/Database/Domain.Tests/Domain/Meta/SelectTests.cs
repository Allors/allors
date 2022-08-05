// <copyright file="PathTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using Database.Data;
    using Xunit;

    public class SelectTests : DomainTest, IClassFixture<Fixture>
    {
        public SelectTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void One2ManyWithPropertyTypes()
        {
            var c2A = this.BuildC2("c2A");
            var c2B = this.BuildC2("c2B");
            var c2C = this.BuildC2("c2C");

            var c1A = this.BuildC1("c1A", v => v.AddC1C2One2Many(c2A));
            var c1B = this.BuildC1("c1B", v => v.C1C2One2Manies = new[] { c2B, c2C });

            this.Transaction.Derive();

            var path = new Select(this.M.C1.C1C2One2Manies, this.M.C2.C2AllorsString);

            var result = (ISet<object>)path.Get(c1A, this.AclsMock.Object);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1B, this.AclsMock.Object);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyTypeIds()
        {
            var c2A = this.BuildC2("c2A");
            var c2B = this.BuildC2("c2B");
            var c2C = this.BuildC2("c2C");

            var c1A = this.BuildC1("c1A", v => v.AddC1C2One2Many(c2A));
            var c1B = this.BuildC1("c1B", v => v.C1C2One2Manies = new[] { c2B, c2C });

            this.Transaction.Derive();

            var path = new Select(this.M.C1.C1C2One2Manies, this.M.C2.C2AllorsString);

            var result = (ISet<object>)path.Get(c1A, this.AclsMock.Object);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1B, this.AclsMock.Object);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyNames()
        {
            var c2A = this.BuildC2("c2A");
            var c2B = this.BuildC2("c2B");
            var c2C = this.BuildC2("c2C");

            var c1A = this.BuildC1("c1A", v => v.AddC1C2One2Many(c2A));
            var c1B = this.BuildC1("c1B", v => v.C1C2One2Manies = new[] { c2B, c2C });

            this.Transaction.Derive();

            Select.TryParse(this.M.C2, "C1WhereC1C2One2Many", out var select);

            var result = (C1)select.Get(c2A, this.AclsMock.Object);
            Assert.Equal(result, c1A);

            result = (C1)select.Get(c2B, this.AclsMock.Object);
            Assert.Equal(result, c1B);
        }
    }
}

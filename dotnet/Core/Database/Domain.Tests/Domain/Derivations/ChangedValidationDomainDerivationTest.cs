// <copyright file="ChangedValidationDomainDerivationTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class ChangedValidationDomainDerivationTest : DomainTest, IClassFixture<Fixture>
    {
        public ChangedValidationDomainDerivationTest(Fixture fixture) : base(fixture, false) { }

        [Fact]
        public void One2One()
        {
            var cc = this.Transaction.Create<CC>();
            var bb = this.Transaction.Create<BB>(v => v.One2One = cc);
            var aa = this.Transaction.Create<AA>(v => v.One2One = bb);

            this.Transaction.Derive();

            cc.Assigned = "x";

            this.Transaction.Derive();

            Assert.Equal("x", aa.Derived);
        }

        [Fact]
        public void Many2One()
        {
            var cc = this.Transaction.Create<CC>();
            var bb = this.Transaction.Create<BB>(v => v.One2One = cc);
            var aa = this.Transaction.Create<AA>(v => v.One2One = bb);

            this.Transaction.Derive();

            cc.Assigned = "x";

            this.Transaction.Derive();

            Assert.Equal("x", aa.Derived);
        }

        [Fact]
        public void One2Many()
        {
            var cc = this.Transaction.Create<CC>();
            var bb = this.Transaction.Create<BB>(v => v.One2One = cc);
            var aa = this.Transaction.Create<AA>(v => v.One2One = bb);

            this.Transaction.Derive();

            cc.Assigned = "x";

            this.Transaction.Derive();

            Assert.Equal("x", aa.Derived);
        }

        [Fact]
        public void Many2Many()
        {
            var cc = this.Transaction.Create<CC>();
            var bb = this.Transaction.Create<BB>(v => v.One2One = cc);
            var aa = this.Transaction.Create<AA>(v => v.One2One = bb);

            this.Transaction.Derive();

            cc.Assigned = "x";

            this.Transaction.Derive();

            Assert.Equal("x", aa.Derived);
        }

        [Fact]
        public void C1ChangedRole()
        {
            var c1 = this.Transaction.Create<C1>();
            var c2 = this.Transaction.Create<C2>();

            c1.ChangedRolePingC1 = true;
            c2.ChangedRolePingC1 = true;

            this.Transaction.Derive();

            Assert.True(c1.ChangedRolePongC1);
            Assert.Null(c2.ChangedRolePongC1);
        }

        [Fact]
        public void I1ChangedRole()
        {
            var c1 = this.Transaction.Create<C1>();
            var c2 = this.Transaction.Create<C2>();

            c1.ChangedRolePingI1 = true;
            c2.ChangedRolePingI1 = true;

            this.Transaction.Derive();

            Assert.True(c1.ChangedRolePongI1);
            Assert.Null(c2.ChangedRolePongI1);
        }

        [Fact]
        public void I12ChangedRole()
        {
            var c1 = this.Transaction.Create<C1>();
            var c2 = this.Transaction.Create<C2>();

            c1.ChangedRolePingI12 = true;
            c2.ChangedRolePingI12 = true;

            this.Transaction.Derive();

            Assert.True(c1.ChangedRolePongI12);
            Assert.True(c2.ChangedRolePongI12);
        }

        [Fact]
        public void S12ChangedRole()
        {
            var c1 = this.Transaction.Create<C1>();
            var c2 = this.Transaction.Create<C2>();

            c1.ChangedRolePingS12 = true;
            c2.ChangedRolePingS12 = true;

            this.Transaction.Derive();

            Assert.True(c1.ChangedRolePongS12);
            Assert.True(c2.ChangedRolePongS12);
        }
    }
}

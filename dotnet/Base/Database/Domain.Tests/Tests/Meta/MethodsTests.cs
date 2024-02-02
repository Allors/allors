// <copyright file="MethodsTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class MethodsTests : DomainTest, IClassFixture<Fixture>
    {
        public MethodsTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void ClassMethod()
        {
            var c1 = this.Transaction.Build<C1>();

            var classMethod = new C1ClassMethod(c1);
            classMethod.Execute();

            Assert.Equal("C1CustomC1Base", classMethod.Value);
        }

        [Fact]
        public void InterfaceMethod()
        {
            var c1 = this.Transaction.Build<C1>();

            var interfaceMethod = new C1InterfaceMethod(c1);
            interfaceMethod.Execute();

            Assert.Equal("I1CustomI1BaseC1CustomC1Base", interfaceMethod.Value);
        }

        [Fact]
        public void SuperinterfaceMethod()
        {
            var c1 = this.Transaction.Build<C1>();

            var superinterfaceMethod = new C1SuperinterfaceMethod(c1);
            superinterfaceMethod.Execute();

            Assert.Equal("S1CustomS1BaseI1CustomI1BaseC1CustomC1Base", superinterfaceMethod.Value);
        }

        [Fact]
        public void CallMethodTwice()
        {
            var c1 = this.Transaction.Build<C1>();

            var classMethod = new C1ClassMethod(c1);
            classMethod.Execute();

            var exceptionThrown = false;
            try
            {
                classMethod.Execute();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }
    }
}

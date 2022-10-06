// <copyright file="MethodsTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

            Assert.Equal("C1CustomC1Core", classMethod.Value);
        }

        [Fact]
        public void InterfaceMethod()
        {
            var c1 = this.Transaction.Build<C1>();

            var interfaceMethod = new C1InterfaceMethod(c1);
            interfaceMethod.Execute();

            Assert.Equal("I1CustomI1CoreC1CustomC1Core", interfaceMethod.Value);
        }

        [Fact]
        public void SuperinterfaceMethod()
        {
            var c1 = this.Transaction.Build<C1>();

            var superinterfaceMethod = new C1SuperinterfaceMethod(c1);
            superinterfaceMethod.Execute();

            Assert.Equal("S1CustomS1CoreI1CustomI1CoreC1CustomC1Core", superinterfaceMethod.Value);
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

        [Fact]
        public void MethodWithResults()
        {
            var c1 = this.Transaction.Build<C1>();

            var output = c1.Sum(new SumInput { ValueA = 1, ValueB = 2 });

            Assert.Equal(3, output.Result);
        }

    }
}

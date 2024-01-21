// <copyright file="PullTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Linq;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;
    using Result = Allors.Workspace.Data.Result;

    public abstract class CacheableTests : Test
    {
        protected CacheableTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public async void UnitRole()
        {
            await this.Login("administrator");
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();

            if (!c1.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
            }

            var counter = 0;

            ChangedEventHandler c1AllorsStringOnChanged = (_, _) => ++counter;

            c1.C1AllorsString.Changed += c1AllorsStringOnChanged;

            c1.C1AllorsString.Value = "+";

            Assert.Equal(1, counter);

            c1.C1AllorsString.Value = "++";

            Assert.Equal(2, counter);

            c1.C1AllorsString.Changed -= c1AllorsStringOnChanged;

            c1.C1AllorsString.Value = "+++";

            Assert.Equal(2, counter);
        }
    }
}

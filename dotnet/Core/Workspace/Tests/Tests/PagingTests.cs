// <copyright file="PullTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Tests.Workspace
{
    using Allors.Workspace.Request;
    using Xunit;

    public abstract class PagingTests : Test
    {
        protected PagingTests(Fixture fixture) : base(fixture) { }


        // Ascending Order
        // c2D -> c2C -> c1B -> c1A -> c2A -> c2B -> c1D -> c1C

        [Fact]
        public async void Take()
        {
            await this.Login("administrator");
            var m = this.M;

            foreach (var connection in this.Connections)
            {
                var pull = new Pull
                {
                    Extent = new Filter(m.I12) { Sorting = new[] { new Sort(m.I12.Order) } },
                    Results = new[]
                    {
                        new Result
                        {
                            Take = 1
                        }
                    }
                };

                var result = await connection.PullAsync(pull);

                var i12s = result.GetCollection(m.I12);

                Assert.Single(i12s);

                Assert.Equal("c2D", i12s[0].GetUnitRole(m.I12.Name));
            }
        }
    }
}

﻿// <copyright file="AssociationTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System.Linq;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Xunit;
    using Result = Allors.Workspace.Data.Result;

    public abstract class AssociationTests : Test
    {
        protected AssociationTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async void Database_GetOne2Many()
        {
            await this.Login("administrator");
            var workspace = this.Workspace;

            var pull = new[]
            {
                new Pull
                {
                    Extent = new Filter(this.M.C2)
                    {
                        Predicate = new Equals(this.M.C2.Name) {Value = "c2C"}
                    },
                    Results = new[]
                    {
                        new Result
                        {
                            Select = new Select
                            {
                                Include = new[] {new Node(this.M.C2.C1WhereC1C2One2Many)}
                            }
                        }
                    }
                }
            };

            var result = await workspace.PullAsync(pull);

            var c2s = result.GetCollection<C2>();

            var c2C = c2s.First(v => v.Name.Value == "c2C");

            var c1WhereC1C2One2Many = c2C.C1WhereC1C2One2Many;

            // One to One
            Assert.NotNull(c1WhereC1C2One2Many);
            Assert.Equal("c1C", c1WhereC1C2One2Many.Value.Name.Value);
        }

        [Fact]
        public async void Database_GetOne2One()
        {
            await this.Login("administrator");
            var workspace = this.Workspace;

            var pull = new[]
            {
                new Pull
                {
                    Extent = new Filter(this.M.C2)
                    {
                        Predicate = new Equals(this.M.C2.Name) {Value = "c2C"}
                    },
                    Results = new[]
                    {
                        new Result
                        {
                            Select = new Select
                            {
                                Include = new[] {new Node(this.M.C2.C1WhereC1C2One2One)}
                            }
                        }
                    }
                }
            };

            var result = await workspace.PullAsync(pull);

            var c2s = result.GetCollection<C2>();

            var c2C = c2s.First(v => v.Name.Value == "c2C");

            var c1WhereC1C2One2One = c2C.C1WhereC1C2One2One;

            // One to One
            Assert.NotNull(c1WhereC1C2One2One);
            Assert.Equal("c1C", c1WhereC1C2One2One.Value.Name.Value);
        }
    }
}

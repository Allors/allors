// <copyright file="ChangeSetTests.cs" company="Allors bv">
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

    public abstract class WorkspaceResetTests : Test
    {
        protected WorkspaceResetTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public async void ResetCreateWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var c1a = workspace.Create<C1>();
            
            this.Workspace.Reset();

            Assert.True(c1a.Strategy.IsDeleted);
        }

        [Fact]
        public async void ResetUnitWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];

            c1a.C1AllorsString.Value = "X";

            await workspace.PushAsync();
            result = await workspace.PullAsync(pull);
            var c2a = result.GetCollection<C1>()[0];

            var c2aString = c2a.C1AllorsString;

            this.Workspace.Reset();

            Assert.Equal(c2aString, c1a.C1AllorsString);

        }

        [Fact]
        public async void ResetUnitAfterPushTest()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];

            c1a.C1AllorsString.Value = "X";

            await workspace.PushAsync();

            Assert.Equal("X", c1a.C1AllorsString.Value);

            this.Workspace.Reset();

            Assert.Null(c1a.C1AllorsString.Value);
        }

        [Fact]
        public async void ResetUnitAfterDoublePush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];

            c1a.C1AllorsString.Value = "X";

            await workspace.PushAsync();
            result = await workspace.PullAsync(pull);
            var c2a = result.GetCollection<C1>()[0];

            c2a.C1AllorsString.Value = "Y";

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.Equal("X", c2a.C1AllorsString.Value);
        }

        [Fact]
        public async void ResetOne2OneWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
                {
                    Predicate = new Equals(this.M.C1.Name) { Value = "c1A" }
                }
            };

            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1x = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1x;

            this.Workspace.Reset();

            Assert.NotNull(Record.Exception(() =>
            {
                var x = c1a.C1C1One2One.Value;
            }));

            Assert.Null(c1x.C1WhereC1C1One2One.Value);
        }

        [Fact]
        public async void ResetOne2OneIncludeWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
                {
                    Predicate = new Equals(this.M.C1.Name) { Value = "c1A" }
                },
                Results =
                [
                    new Result
                    {
                        Include = [new Node(this.M.C1.C1C1One2One)]
                    }
                ]
            };

            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = c1a.C1C1One2One.Value;
            var c1x = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1x;

            this.Workspace.Reset();

            Assert.Equal(c1b, c1a.C1C1One2One.Value);
            Assert.Null(c1x.C1WhereC1C1One2One.Value);
        }

        [Fact]
        public async void ResetOne2OneAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1b;

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.NotNull(Record.Exception(() =>
            {
                var x = c1a.C1C1One2One.Value;
            }));

            Assert.Null(c1b.C1WhereC1C1One2One.Value);
        }

        [Fact]
        public async void ResetOne2OneIncludeAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
                {
                    Predicate = new Equals(this.M.C1.Name) { Value = "c1A" }
                },
                Results =
                [
                    new Result
                    {
                        Include = [new Node(this.M.C1.C1C1One2One)]
                    }
                ]
            };

            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = c1a.C1C1One2One.Value;
            var c1x = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1x;

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.Equal(c1b, c1a.C1C1One2One.Value);
            Assert.Null(c1x.C1WhereC1C1One2One.Value);
        }

        [Fact]
        public async void ResetOne2OneRemoveAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1b;

            Assert.Equal(c1b, c1a.C1C1One2One.Value);
            Assert.Equal(c1a, c1b.C1WhereC1C1One2One.Value);

            await workspace.PushAsync();
            result = await workspace.PullAsync(pull);

            c1a.C1C1One2One.Value = null;

            Assert.Null(c1a.C1C1One2One.Value);
            Assert.Null(c1b.C1WhereC1C1One2One.Value);

            this.Workspace.Reset();

            Assert.Equal(c1b, c1a.C1C1One2One.Value);
            Assert.Equal(c1a, c1b.C1WhereC1C1One2One.Value);
        }

        [Fact]
        public async void ResetMany2OneWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1Many2One.Value = c1b;

            this.Workspace.Reset();

            Assert.Null(c1a.C1C1Many2One.Value);
            Assert.Empty(c1b.C1sWhereC1C1Many2One.Value);
        }

        [Fact]
        public async void ResetMany2OneAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1Many2One.Value = c1b;

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.Null(c1a.C1C1Many2One.Value);
            Assert.Empty(c1b.C1sWhereC1C1Many2One.Value);
        }

        [Fact]
        public async void ResetMany2OneRemoveAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1Many2One.Value = c1b;

            Assert.Equal(c1b, c1a.C1C1Many2One.Value);
            Assert.Contains(c1a, c1b.C1sWhereC1C1Many2One.Value);

            await workspace.PushAsync();
            result = await workspace.PullAsync(pull);
            
            Assert.Equal(c1b, c1a.C1C1Many2One.Value);
            Assert.Contains(c1a, c1b.C1sWhereC1C1Many2One.Value);

            c1a.C1C1Many2One.Value = null;

            Assert.Null(c1a.C1C1Many2One.Value);
            Assert.Empty(c1b.C1sWhereC1C1Many2One.Value);

            this.Workspace.Reset();

            Assert.Equal(c1b, c1a.C1C1Many2One.Value);
            Assert.Contains(c1a, c1b.C1sWhereC1C1Many2One.Value);
        }

        [Fact]
        public async void ResetOne2ManyWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1One2Manies.Add(c1b);

            this.Workspace.Reset();

            Assert.Empty(c1a.C1C1One2Manies.Value);
            Assert.Null(c1b.C1WhereC1C1One2Many.Value);
        }

        [Fact]
        public async void ResetOne2ManyAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1One2Manies.Add(c1b);

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.Empty(c1a.C1C1One2Manies.Value);
            Assert.Null(c1b.C1WhereC1C1One2Many.Value);
        }

        [Fact]
        public async void ResetOne2ManyRemoveAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            await workspace.PushAsync();
            result = await workspace.PullAsync(new Pull { Extent = new Filter(M.C1) });

            c1a.C1C1One2Manies.Add(c1b);

            await workspace.PushAsync();
            await workspace.PullAsync(pull);

            c1a.C1C1One2Manies.Remove(c1b);

            this.Workspace.Reset();

            Assert.Contains(c1b, c1a.C1C1One2Manies.Value);
            Assert.Equal(c1a, c1b.C1WhereC1C1One2Many.Value);
        }

        [Fact]
        public async void ResetMany2ManyWithoutPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1Many2Manies.Add(c1b);

            this.Workspace.Reset();

            Assert.Empty(c1a.C1C1Many2Manies.Value);
            Assert.Empty(c1b.C1sWhereC1C1Many2Many.Value);
        }

        [Fact]
        public async void ResetMany2ManyAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            c1a.C1C1Many2Manies.Add(c1b);

            await workspace.PushAsync();

            this.Workspace.Reset();

            Assert.Empty(c1a.C1C1Many2Manies.Value);
            Assert.Empty(c1b.C1sWhereC1C1Many2Many.Value);
        }

        [Fact]
        public async void ResetMany2ManyRemoveAfterPush()
        {
            await this.Login("administrator");

            var workspace = this.Workspace;

            var pull = new Pull { Extent = new Filter(this.M.C1) { Predicate = new Equals(this.M.C1.Name) { Value = "c1A" } } };
            var result = await workspace.PullAsync(pull);
            var c1a = result.GetCollection<C1>()[0];
            var c1b = workspace.Create<C1>();

            await workspace.PushAsync();
            result = await workspace.PullAsync(new Pull { Object = c1b.Strategy });
            var c1b_2 = result.Objects.Values.First().Cast<C1>();

            c1a.C1C1Many2Manies.Add(c1b_2);

            Assert.Contains(c1b_2, c1a.C1C1Many2Manies.Value);
            Assert.Contains(c1a, c1b_2.C1sWhereC1C1Many2Many.Value);

            await workspace.PushAsync();
            result = await workspace.PullAsync(pull);
            c1a = result.GetCollection<C1>()[0];

            c1a.C1C1Many2Manies.Remove(c1b_2);

            Assert.Empty(c1a.C1C1Many2Manies.Value);
            Assert.Empty(c1b_2.C1sWhereC1C1Many2Many.Value);

            this.Workspace.Reset();

            Assert.Contains(c1b, c1a.C1C1Many2Manies.Value);
            Assert.Contains(c1a, c1b.C1sWhereC1C1Many2Many.Value);
        }
    }
}

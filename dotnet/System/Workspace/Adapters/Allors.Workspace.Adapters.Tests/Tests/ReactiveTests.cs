// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Allors.Workspace.Domain;
    using Xunit;
    using Allors.Workspace.Data;
    using Allors.Workspace;

    public abstract class ReactiveTests : Test
    {
        private Func<Context>[] contextFactories;

        protected ReactiveTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");

            var singleWorkspaceContext = new SingleWorkspaceContext(this, "Single Shared Workspace");
            var multipleWorkspaceContext = new MultipleWorkspaceContext(this, "Multiple Shared Workspace");

            this.contextFactories = new Func<Context>[]
            {
                () => singleWorkspaceContext,
                () => new SingleWorkspaceContext(this, "Single Workspace"),
                () => multipleWorkspaceContext,
                () => new MultipleWorkspaceContext(this, "Multiple Workspace"),
            };
        }

        [Fact]
        public async void StringRole()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1 = await ctx.Create<C1>(workspace1, mode);
                    if (!c1.C1AllorsString.CanWrite)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1.Strategy });
                    }

                    var propertyChanges = new List<string>();

                    c1.C1AllorsString.PropertyChanged += (sender, args) =>
                    {
                        propertyChanges.Add(args.PropertyName);
                    };

                    c1.C1AllorsString.Value = null;

                    Assert.Empty(propertyChanges);

                    c1.C1AllorsString.Value = null;

                    Assert.Empty(propertyChanges);

                    c1.C1AllorsString.Value = "Hello world!";

                    Assert.Equal(3, propertyChanges.Count);
                    Assert.Contains("Value", propertyChanges);
                    Assert.Contains("Exist", propertyChanges);
                    Assert.Contains("IsModified", propertyChanges);
                }
            }
        }

        [Fact]
        public async void OneToOneRole()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1a = await ctx.Create<C1>(workspace1, mode);
                    var c1b = await ctx.Create<C1>(workspace1, mode);
                    var c1c = await ctx.Create<C1>(workspace1, mode);

                    if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite)
                    {
                        await workspace1.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy });
                    }

                    c1a.C1C1One2One.Value = c1b;
                    c1b.C1C1One2One.Value = c1c;

                    var c1aPropertyChanges = new List<string>();
                    var c1bPropertyChanges = new List<string>();

                    c1a.C1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1aPropertyChanges.Add(args.PropertyName);
                    };

                    c1b.C1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1bPropertyChanges.Add(args.PropertyName);
                    };

                    c1b.C1C1One2One.Value = c1c;

                    Assert.Empty(c1bPropertyChanges);

                    c1b.C1C1One2One.Value = c1c;

                    Assert.Empty(c1bPropertyChanges);

                    c1b.C1C1One2One.Value = c1b;

                    Assert.Equal(2, c1aPropertyChanges.Count);
                    Assert.Contains("Value", c1aPropertyChanges);
                    Assert.Contains("Exist", c1aPropertyChanges);
                    Assert.Single(c1bPropertyChanges);
                    Assert.Contains("Value", c1bPropertyChanges);
                }
            }
        }
    }
}

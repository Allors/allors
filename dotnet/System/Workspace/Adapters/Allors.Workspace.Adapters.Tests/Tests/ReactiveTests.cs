// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public async void SetString()
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
        public async void SetOneToOne()
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

                    var c1bAssociationPropertyChanges = new List<string>();
                    var c1cAssociationPropertyChanges = new List<string>();

                    c1a.C1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1aPropertyChanges.Add(args.PropertyName);
                    };

                    c1b.C1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1bPropertyChanges.Add(args.PropertyName);
                    };

                    c1b.C1WhereC1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1bAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1c.C1WhereC1C1One2One.PropertyChanged += (sender, args) =>
                    {
                        c1cAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1b.C1C1One2One.Value = c1c;

                    Assert.Empty(c1bPropertyChanges);

                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);

                    c1b.C1C1One2One.Value = c1c;

                    Assert.Empty(c1bPropertyChanges);

                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);

                    c1b.C1C1One2One.Value = c1b;

                    Assert.Equal(2, c1aPropertyChanges.Count);
                    Assert.Contains("Value", c1aPropertyChanges);
                    Assert.Contains("Exist", c1aPropertyChanges);
                    Assert.Single(c1bPropertyChanges);
                    Assert.Contains("Value", c1bPropertyChanges);

                    Assert.Single(c1bAssociationPropertyChanges);
                    Assert.Contains("Value", c1bAssociationPropertyChanges);
                    Assert.Equal(1, c1cAssociationPropertyChanges.Count);
                    Assert.Contains("Value", c1cAssociationPropertyChanges);
                }
            }
        }

        [Fact]
        public async void SetManyToOne()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1A = await ctx.Create<C1>(workspace1, mode);
                    var c1B = await ctx.Create<C1>(workspace1, mode);
                    var c1C = await ctx.Create<C1>(workspace1, mode);
                    var c1D = await ctx.Create<C1>(workspace1, mode);

                    if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
                    {
                        var pull = new[] { c1A, c1B, c1C };
                        await workspace1.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
                    }

                    c1B.C1C1Many2One.Value = c1B;
                    c1C.C1C1Many2One.Value = c1C;
                    c1D.C1C1Many2One.Value = c1C;

                    c1B.C1C1Many2One.Value = c1A;

                    var c1aRolePropertyChanges = new List<string>();
                    var c1bRolePropertyChanges = new List<string>();
                    var c1cRolePropertyChanges = new List<string>();
                    var c1dRolePropertyChanges = new List<string>();

                    var c1aAssociationPropertyChanges = new List<string>();
                    var c1bAssociationPropertyChanges = new List<string>();
                    var c1cAssociationPropertyChanges = new List<string>();
                    var c1dAssociationPropertyChanges = new List<string>();

                    c1A.C1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1aRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1bRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1cRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1dRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1A.C1sWhereC1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1aAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1sWhereC1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1bAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1sWhereC1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1cAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1sWhereC1C1Many2One.PropertyChanged += (sender, args) =>
                    {
                        c1dAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1C1Many2One.Value = c1A;

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1B.C1C1Many2One.Value = c1A;

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1B.C1C1Many2One.Value = c1B;

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Single(c1bRolePropertyChanges);
                    Assert.Contains("Value", c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Single(c1aAssociationPropertyChanges);
                    Assert.Contains("Value", c1aAssociationPropertyChanges);
                    Assert.Single(c1bAssociationPropertyChanges);
                    Assert.Contains("Value", c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1B.C1C1Many2One.Value = c1B;

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Single(c1bRolePropertyChanges);
                    Assert.Contains("Value", c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Single(c1aAssociationPropertyChanges);
                    Assert.Contains("Value", c1aAssociationPropertyChanges);
                    Assert.Single(c1bAssociationPropertyChanges);
                    Assert.Contains("Value", c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);
                }
            }
        }

        [Fact]
        public async void AddOneToMany()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1A = await ctx.Create<C1>(workspace1, mode);
                    var c1B = await ctx.Create<C1>(workspace1, mode);
                    var c1C = await ctx.Create<C1>(workspace1, mode);
                    var c1D = await ctx.Create<C1>(workspace1, mode);

                    if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
                    {
                        var pull = new[] { c1A, c1B, c1C };
                        await workspace1.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
                    }

                    c1B.C1C1One2Manies.Add(c1B);
                    c1C.C1C1One2Manies.Add(c1C);
                    c1C.C1C1One2Manies.Add(c1D);

                    var c1aRolePropertyChanges = new List<string>();
                    var c1bRolePropertyChanges = new List<string>();
                    var c1cRolePropertyChanges = new List<string>();
                    var c1dRolePropertyChanges = new List<string>();

                    var c1aAssociationPropertyChanges = new List<string>();
                    var c1bAssociationPropertyChanges = new List<string>();
                    var c1cAssociationPropertyChanges = new List<string>();
                    var c1dAssociationPropertyChanges = new List<string>();

                    c1A.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1aRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1bRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1cRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1dRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1A.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1aAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1bAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1cAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1dAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1One2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1One2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1B.C1C1One2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Single(c1bRolePropertyChanges);
                    Assert.Contains("Value", c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Single(c1cAssociationPropertyChanges);
                    Assert.Contains("Value", c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);
                    
                    c1B.C1C1One2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Single(c1bRolePropertyChanges);
                    Assert.Contains("Value", c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Single(c1cAssociationPropertyChanges);
                    Assert.Contains("Value", c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);
                }
            }
        }

        [Fact]
        public async void RemoveOneToMany()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1A = await ctx.Create<C1>(workspace1, mode);
                    var c1B = await ctx.Create<C1>(workspace1, mode);
                    var c1C = await ctx.Create<C1>(workspace1, mode);
                    var c1D = await ctx.Create<C1>(workspace1, mode);

                    if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
                    {
                        var pull = new[] { c1A, c1B, c1C };
                        await workspace1.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
                    }

                    c1B.C1C1One2Manies.Add(c1B);
                    c1C.C1C1One2Manies.Add(c1C);
                    c1C.C1C1One2Manies.Add(c1D);

                    var c1aRolePropertyChanges = new List<string>();
                    var c1bRolePropertyChanges = new List<string>();
                    var c1cRolePropertyChanges = new List<string>();
                    var c1dRolePropertyChanges = new List<string>();

                    var c1aAssociationPropertyChanges = new List<string>();
                    var c1bAssociationPropertyChanges = new List<string>();
                    var c1cAssociationPropertyChanges = new List<string>();
                    var c1dAssociationPropertyChanges = new List<string>();

                    c1A.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1aRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1bRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1cRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1C1One2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1dRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1A.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1aAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1bAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1cAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1WhereC1C1One2Many.PropertyChanged += (sender, args) =>
                    {
                        c1dAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1One2Manies.Remove(c1B);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1One2Manies.Remove(c1B);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1One2Manies.Remove(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Single(c1cAssociationPropertyChanges);
                    Assert.Contains("Value", c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1One2Manies.Remove(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Single(c1cAssociationPropertyChanges);
                    Assert.Contains("Value", c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);
                }
            }
        }

        [Fact]
        public async void AddManyToMany()
        {
            foreach (DatabaseMode mode in Enum.GetValues(typeof(DatabaseMode)))
            {
                foreach (var contextFactory in this.contextFactories)
                {
                    var ctx = contextFactory();
                    var (workspace1, _) = ctx;

                    var c1A = await ctx.Create<C1>(workspace1, mode);
                    var c1B = await ctx.Create<C1>(workspace1, mode);
                    var c1C = await ctx.Create<C1>(workspace1, mode);
                    var c1D = await ctx.Create<C1>(workspace1, mode);

                    if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
                    {
                        var pull = new[] { c1A, c1B, c1C };
                        await workspace1.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
                    }

                    c1B.C1C1Many2Manies.Add(c1B);
                    c1C.C1C1Many2Manies.Add(c1B);
                    c1D.C1C1Many2Manies.Add(c1B);
                    c1C.C1C1Many2Manies.Add(c1C);
                    c1D.C1C1Many2Manies.Add(c1C);
                    c1D.C1C1Many2Manies.Add(c1D);

                    var c1aRolePropertyChanges = new List<string>();
                    var c1bRolePropertyChanges = new List<string>();
                    var c1cRolePropertyChanges = new List<string>();
                    var c1dRolePropertyChanges = new List<string>();

                    var c1aAssociationPropertyChanges = new List<string>();
                    var c1bAssociationPropertyChanges = new List<string>();
                    var c1cAssociationPropertyChanges = new List<string>();
                    var c1dAssociationPropertyChanges = new List<string>();

                    c1A.C1C1Many2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1aRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1C1Many2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1bRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1Many2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1cRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1C1Many2Manies.PropertyChanged += (sender, args) =>
                    {
                        c1dRolePropertyChanges.Add(args.PropertyName);
                    };

                    c1A.C1sWhereC1C1Many2Many.PropertyChanged += (sender, args) =>
                    {
                        c1aAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1B.C1sWhereC1C1Many2Many.PropertyChanged += (sender, args) =>
                    {
                        c1bAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1sWhereC1C1Many2Many.PropertyChanged += (sender, args) =>
                    {
                        c1cAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1D.C1sWhereC1C1Many2Many.PropertyChanged += (sender, args) =>
                    {
                        c1dAssociationPropertyChanges.Add(args.PropertyName);
                    };

                    c1C.C1C1Many2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1Many2Manies.Add(c1C);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Empty(c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Empty(c1dAssociationPropertyChanges);

                    c1C.C1C1Many2Manies.Add(c1D);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Single(c1dAssociationPropertyChanges);
                    Assert.Contains("Value", c1dAssociationPropertyChanges);

                    c1C.C1C1Many2Manies.Add(c1D);

                    Assert.Empty(c1aRolePropertyChanges);
                    Assert.Empty(c1bRolePropertyChanges);
                    Assert.Single(c1cRolePropertyChanges);
                    Assert.Contains("Value", c1cRolePropertyChanges);
                    Assert.Empty(c1dRolePropertyChanges);

                    Assert.Empty(c1aAssociationPropertyChanges);
                    Assert.Empty(c1bAssociationPropertyChanges);
                    Assert.Empty(c1cAssociationPropertyChanges);
                    Assert.Single(c1dAssociationPropertyChanges);
                    Assert.Contains("Value", c1dAssociationPropertyChanges);
                }
            }
        }
    }
}

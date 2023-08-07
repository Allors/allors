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
        protected ReactiveTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void SetString()
        {
            var workspace = this.Profile.Workspace;

            var c1 = workspace.Create<C1>();
            if (!c1.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
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

        [Fact]
        public async void SetOneToOne()
        {
            var workspace1 = this.Profile.Workspace;

            var c1a = workspace1.Create<C1>();
            var c1b = workspace1.Create<C1>();
            var c1c = workspace1.Create<C1>();

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
            Assert.Single(c1cAssociationPropertyChanges);
            Assert.Contains("Value", c1cAssociationPropertyChanges);
        }

        [Fact]
        public async void SetManyToOne()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1A, c1B, c1C };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
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

        [Fact]
        public async void AddOneToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1A, c1B, c1C };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
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

        [Fact]
        public async void RemoveOneToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1A, c1B, c1C };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
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

        [Fact]
        public async void AddManyToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1A, c1B, c1C };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
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

        [Fact]
        public async void RemoveManyToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1A = workspace.Create<C1>();
            var c1B = workspace.Create<C1>();
            var c1C = workspace.Create<C1>();
            var c1D = workspace.Create<C1>();

            if (!c1A.C1C1Many2One.CanWrite || !c1B.C1C1Many2One.CanWrite || !c1C.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1A, c1B, c1C };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
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

            c1C.C1C1Many2Manies.Remove(c1A);

            Assert.Empty(c1aRolePropertyChanges);
            Assert.Empty(c1bRolePropertyChanges);
            Assert.Empty(c1cRolePropertyChanges);
            Assert.Empty(c1dRolePropertyChanges);

            Assert.Empty(c1aAssociationPropertyChanges);
            Assert.Empty(c1bAssociationPropertyChanges);
            Assert.Empty(c1cAssociationPropertyChanges);
            Assert.Empty(c1dAssociationPropertyChanges);

            c1C.C1C1Many2Manies.Remove(c1A);

            Assert.Empty(c1aRolePropertyChanges);
            Assert.Empty(c1bRolePropertyChanges);
            Assert.Empty(c1cRolePropertyChanges);
            Assert.Empty(c1dRolePropertyChanges);

            Assert.Empty(c1aAssociationPropertyChanges);
            Assert.Empty(c1bAssociationPropertyChanges);
            Assert.Empty(c1cAssociationPropertyChanges);
            Assert.Empty(c1dAssociationPropertyChanges);

            c1C.C1C1Many2Manies.Remove(c1C);

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

            c1C.C1C1Many2Manies.Remove(c1C);

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

        [Fact]
        public async void PullString()
        {
            var workspaceX = this.Profile.CreateSharedWorkspace();
            var workspaceY = this.Profile.CreateExclusiveWorkspace();

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
                {
                    Predicate = new Equals { PropertyType = this.M.C1.Name, Value = "c1A" }
                }
            };

            var xResult = await workspaceX.PullAsync(pull);
            var xC1A = xResult.GetCollection<C1>().First();

            var propertyChanges = new List<string>();

            xC1A.C1AllorsString.PropertyChanged += (sender, args) =>
            {
                propertyChanges.Add(args.PropertyName);
            };

            var yResult = await workspaceY.PullAsync(pull);
            var yC1A = yResult.GetCollection<C1>().First();

            yC1A.C1AllorsString.Value = "New New New";

            await workspaceY.PushAsync();

            await workspaceX.PullAsync(pull);

            Assert.Equal("New New New", yC1A.C1AllorsString.Value);

            Assert.Equal(2, propertyChanges.Count);
            Assert.Contains("Value", propertyChanges);
            Assert.Contains("Exist", propertyChanges);
        }

        [Fact]
        public async void PullOneToOne()
        {
            var workspaceX = this.Profile.CreateSharedWorkspace();
            var workspaceY = this.Profile.CreateExclusiveWorkspace();

            var pull = new Pull
            {
                Extent = new Filter(this.M.C1)
            };

            var xResult = await workspaceX.PullAsync(pull);
            var xC1A = xResult.GetCollection<C1>().First(v => v.Name.Value == "c1A");
            var xC1B = xResult.GetCollection<C1>().First(v => v.Name.Value == "c1B");

            var c1ARolePropertyChanges = new List<string>();
            var c1BRolePropertyChanges = new List<string>();

            var c1AAssociationPropertyChanges = new List<string>();
            var c1BAssociationPropertyChanges = new List<string>();

            xC1A.C1C1One2One.PropertyChanged += (sender, args) =>
            {
                c1ARolePropertyChanges.Add(args.PropertyName);
            };

            xC1B.C1C1One2One.PropertyChanged += (sender, args) =>
            {
                c1BRolePropertyChanges.Add(args.PropertyName);
            };

            xC1A.C1WhereC1C1One2One.PropertyChanged += (sender, args) =>
            {
                c1AAssociationPropertyChanges.Add(args.PropertyName);
            };

            xC1B.C1WhereC1C1One2One.PropertyChanged += (sender, args) =>
            {
                c1BAssociationPropertyChanges.Add(args.PropertyName);
            };

            var yResult = await workspaceY.PullAsync(pull);
            var yC1A = yResult.GetCollection<C1>().First(v => v.Name.Value == "c1A");
            var yC1B = yResult.GetCollection<C1>().First(v => v.Name.Value == "c1B");

            yC1B.C1C1One2One.Value = yC1B;

            await workspaceY.PushAsync();

            await workspaceX.PullAsync(pull);

            Assert.Null(xC1A.C1C1One2One.Value);
            Assert.Equal(xC1B, xC1B.C1C1One2One.Value);

            Assert.Equal(2, c1ARolePropertyChanges.Count);
            Assert.Contains("Value", c1ARolePropertyChanges);
            Assert.Contains("Exist", c1ARolePropertyChanges);

            Assert.Single(c1BRolePropertyChanges);
            Assert.Contains("Value", c1BRolePropertyChanges);

            Assert.Empty(c1AAssociationPropertyChanges);

            Assert.Single(c1BAssociationPropertyChanges);
            Assert.Contains("Value", c1BAssociationPropertyChanges);
        }
    }
}

﻿// <copyright file="Many2OneTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
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

        public override async System.Threading.Tasks.Task InitializeAsync()
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
            
            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            Assert.Equal(0, c1.C1AllorsString.Version);

            c1.C1AllorsString.Value = null;

            Assert.Empty(changes);
            Assert.Equal(0, c1.C1AllorsString.Version);

            c1.C1AllorsString.Value = null;

            Assert.Empty(changes);
            Assert.Equal(0, c1.C1AllorsString.Version);

            c1.C1AllorsString.Value = "Hello world!";

            Assert.Single(changes);
            Assert.Contains(c1.C1AllorsString, changes);
            Assert.Equal(1, c1.C1AllorsString.Version);
        }

        [Fact]
        public async void SetOneToOne()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy }, new Pull { Object = c1c.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1c.C1C1One2One.Value = c1d;

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1One2One.Value = c1b;

            Assert.Empty(changes);

            c1c.C1C1One2One.Value = c1d;

            Assert.Empty(changes);

            /*  [given]              [when set]            [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a *   --* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1b ---    c1d        c1c *--   * c1d
             *
             */

            c1c.C1C1One2One.Value = c1b;

            Assert.Equal(4, changes.Count);
            Assert.Contains(c1a.C1C1One2One, changes);
            Assert.Contains(c1b.C1WhereC1C1One2One, changes);
            Assert.Contains(c1c.C1C1One2One, changes);
            Assert.Contains(c1d.C1WhereC1C1One2One, changes);
        }

        [Fact]
        public async void SetManyToOne()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1Many2One.CanWrite || !c1b.C1C1Many2One.CanWrite || !c1c.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1a, c1b, c1c };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
            }

            c1a.C1C1Many2One.Value = c1b;
            c1c.C1C1Many2One.Value = c1d;

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1Many2One.Value = c1b;

            Assert.Empty(changes);

            c1c.C1C1Many2One.Value = c1d;

            Assert.Empty(changes);

            /*  [given]               [when set]            [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *                    +          -          =          -       
             *  c1c ------- c1d       c1c ---     c1d       c1c *--   * c1d
             *
             */

            c1c.C1C1Many2One.Value = c1b;

            Assert.Equal(3, changes.Count);
            Assert.Contains(c1c.C1C1Many2One, changes);
            Assert.Contains(c1b.C1sWhereC1C1Many2One, changes);
            Assert.Contains(c1d.C1sWhereC1C1Many2One, changes);
        }

        [Fact]
        public async void AddOneToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1Many2One.CanWrite || !c1b.C1C1Many2One.CanWrite || !c1c.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1a, c1b, c1c };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
            }

            c1a.C1C1One2Manies.Add(c1b);
            c1c.C1C1One2Manies.Add(c1d);

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1One2Manies.Add(c1b);

            Assert.Empty(changes);

            c1c.C1C1One2Manies.Add(c1d);

            Assert.Empty(changes);

            /*  [given]               [when added]          [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a *   --* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Add(c1b);

            Assert.Equal(3, changes.Count);
            Assert.Contains(c1c.C1C1One2Manies, changes);
            Assert.Contains(c1b.C1WhereC1C1One2Many, changes);
            Assert.Contains(c1a.C1C1One2Manies, changes);
        }

        [Fact]
        public async void RemoveOneToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1Many2One.CanWrite || !c1b.C1C1Many2One.CanWrite || !c1c.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1a, c1b, c1c };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
            }

            c1a.C1C1One2Manies.Add(c1b);
            c1c.C1C1One2Manies.Add(c1b);
            c1c.C1C1One2Manies.Add(c1d);

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1One2Manies.Remove(c1c);

            Assert.Empty(changes);

            c1c.C1C1One2Manies.Remove(c1a);

            Assert.Empty(changes);

            /*  [given]               [when removed]        [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a -------* c1b
             *         -          -          -          =          
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Remove(c1b);

            Assert.Equal(2, changes.Count);
            Assert.Contains(c1c.C1C1One2Manies, changes);
            Assert.Contains(c1b.C1WhereC1C1One2Many, changes);
        }

        [Fact]
        public async void AddManyToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1Many2One.CanWrite || !c1b.C1C1Many2One.CanWrite || !c1c.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1a, c1b, c1c };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
            }

            c1a.C1C1Many2Manies.Add(c1b);
            c1c.C1C1Many2Manies.Add(c1d);

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1Many2Manies.Add(c1b);

            Assert.Empty(changes);

            c1c.C1C1Many2Manies.Add(c1d);

            Assert.Empty(changes);

            /*  [given]               [when added]          [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Add(c1b);

            Assert.Equal(2, changes.Count);
            Assert.Contains(c1c.C1C1One2Manies, changes);
            Assert.Contains(c1b.C1WhereC1C1One2Many, changes);
        }

        [Fact]
        public async void RemoveManyToMany()
        {
            var workspace = this.Profile.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1Many2One.CanWrite || !c1b.C1C1Many2One.CanWrite || !c1c.C1C1Many2One.CanWrite)
            {
                var pull = new[] { c1a, c1b, c1c };
                await workspace.PullAsync(pull.Select(v => new Pull { Object = v.Strategy }).ToArray());
            }

            c1a.C1C1Many2Manies.Add(c1b);
            c1c.C1C1One2Manies.Add(c1b);
            c1c.C1C1Many2Manies.Add(c1d);

            var changes = new HashSet<IOperand>();

            workspace.WorkspaceChanged += (sender, args) =>
            {
                changes.UnionWith(args.Operands);
            };

            c1a.C1C1Many2Manies.Remove(c1c);

            Assert.Empty(changes);

            c1c.C1C1Many2Manies.Remove(c1a);

            Assert.Empty(changes);

            /*  [given]               [when removed]        [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *         -          +          -          =           
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Remove(c1b);

            Assert.Equal(2, changes.Count);
            Assert.Contains(c1c.C1C1One2Manies, changes);
            Assert.Contains(c1b.C1WhereC1C1One2Many, changes);
        }


        [Fact]
        public async void PullString()
        {
            var workspaceX = this.Workspace;
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

            int databaseChanges = 0;
            int workspaceChanges = 0;

            workspaceX.DatabaseChanged += (sender, args) =>
            {
                databaseChanges++;
            };

            workspaceX.WorkspaceChanged += (sender, args) =>
            {
                workspaceChanges++;
            };

            var yResult = await workspaceY.PullAsync(pull);
            var yC1A = yResult.GetCollection<C1>().First();

            yC1A.C1AllorsString.Value = "New New New";

            await workspaceY.PushAsync();

            await workspaceX.PullAsync(pull);

            Assert.Equal("New New New", yC1A.C1AllorsString.Value);

            Assert.Equal(1, databaseChanges);
            Assert.Equal(0, workspaceChanges);
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

            int databaseChanges = 0;
            int workspaceChanges = 0;

            workspaceX.DatabaseChanged += (sender, args) =>
            {
                databaseChanges++;
            };

            workspaceX.WorkspaceChanged += (sender, args) =>
            {
                workspaceChanges++;
            };

            var yResult = await workspaceY.PullAsync(pull);
            var yC1A = yResult.GetCollection<C1>().First(v => v.Name.Value == "c1A");
            var yC1B = yResult.GetCollection<C1>().First(v => v.Name.Value == "c1B");

            yC1B.C1C1One2One.Value = yC1B;

            await workspaceY.PushAsync();

            await workspaceX.PullAsync(pull);

            Assert.Null(xC1A.C1C1One2One.Value);
            Assert.Equal(xC1B, xC1B.C1C1One2One.Value);
            
            Assert.Equal(1, databaseChanges);
            Assert.Equal(0, workspaceChanges);
        }
    }
}

// <copyright file="Many2OneTests.cs" company="Allors bv">
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

            int operandChanged = 0;
            int workspaceChanged = 0;

            c1.C1AllorsString.InvalidationRequested += (sender, args) =>
            {
                ++operandChanged;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };

            c1.C1AllorsString.Value = null;

            Assert.Equal(0, operandChanged);
            Assert.Equal(1, workspaceChanged);

            c1.C1AllorsString.Value = null;

            Assert.Equal(0, operandChanged);
            Assert.Equal(2, workspaceChanged);

            c1.C1AllorsString.Value = "Hello world!";

            Assert.Equal(1, operandChanged);
            Assert.Equal(3, workspaceChanged);
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

            #region signals
            int c1a_C1C1One2One = 0;
            int c1b_C1WhereC1C1One2One = 0;
            int c1c_C1C1One2One = 0;
            int c1d_C1WhereC1C1One2One = 0;
            int workspaceChanged = 0;

            c1a.C1C1One2One.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1One2One;
            };

            c1b.C1WhereC1C1One2One.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1WhereC1C1One2One;
            };

            c1c.C1C1One2One.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1One2One;
            };

            c1d.C1WhereC1C1One2One.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1WhereC1C1One2One;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1One2One.Value = c1b;

            Assert.Equal(0, c1a_C1C1One2One);
            Assert.Equal(0, c1b_C1WhereC1C1One2One);
            Assert.Equal(0, c1c_C1C1One2One);
            Assert.Equal(0, c1c_C1C1One2One);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1One2One.Value = c1d;

            Assert.Equal(0, c1a_C1C1One2One);
            Assert.Equal(0, c1b_C1WhereC1C1One2One);
            Assert.Equal(0, c1c_C1C1One2One);
            Assert.Equal(0, c1c_C1C1One2One);
            Assert.Equal(2, workspaceChanged);

            /*  [given]              [when set]            [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a *   --* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1b ---    c1d        c1c *--   * c1d
             *
             */

            c1c.C1C1One2One.Value = c1b;

            Assert.Equal(1, c1a_C1C1One2One);
            Assert.Equal(1, c1b_C1WhereC1C1One2One);
            Assert.Equal(1, c1c_C1C1One2One);
            Assert.Equal(1, c1c_C1C1One2One);
            Assert.Equal(3, workspaceChanged);
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

            #region signals
            int c1a_C1C1Many2One = 0;
            int c1b_C1sWhereC1C1Many2One = 0;
            int c1c_C1C1Many2One = 0;
            int c1d_C1sWhereC1C1Many2One = 0;
            int workspaceChanged = 0;

            c1a.C1C1Many2One.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1Many2One;
            };

            c1b.C1sWhereC1C1Many2One.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1sWhereC1C1Many2One;
            };

            c1c.C1C1Many2One.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1Many2One;
            };

            c1d.C1sWhereC1C1Many2One.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1sWhereC1C1Many2One;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1Many2One.Value = c1b;

            Assert.Equal(0, c1a_C1C1Many2One);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2One);
            Assert.Equal(0, c1c_C1C1Many2One);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2One);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1Many2One.Value = c1d;

            Assert.Equal(0, c1a_C1C1Many2One);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2One);
            Assert.Equal(0, c1c_C1C1Many2One);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2One);
            Assert.Equal(2, workspaceChanged);

            /*  [given]               [when set]            [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *                    +          -          =          -       
             *  c1c ------- c1d       c1c ---     c1d       c1c *--   * c1d
             *
             */

            c1c.C1C1Many2One.Value = c1b;

            Assert.Equal(0, c1a_C1C1Many2One);
            Assert.Equal(1, c1b_C1sWhereC1C1Many2One);
            Assert.Equal(1, c1c_C1C1Many2One);
            Assert.Equal(1, c1d_C1sWhereC1C1Many2One);
            Assert.Equal(3, workspaceChanged);
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

            #region signals
            int c1a_C1C1One2Manies = 0;
            int c1b_C1WhereC1C1One2Many = 0;
            int c1c_C1C1One2Manies = 0;
            int c1d_C1WhereC1C1One2Many = 0;
            int workspaceChanged = 0;

            c1a.C1C1One2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1One2Manies;
            };

            c1b.C1WhereC1C1One2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1WhereC1C1One2Many;
            };

            c1c.C1C1One2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1One2Manies;
            };

            c1d.C1WhereC1C1One2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1WhereC1C1One2Many;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1One2Manies.Add(c1b);

            Assert.Equal(0, c1a_C1C1One2Manies);
            Assert.Equal(0, c1b_C1WhereC1C1One2Many);
            Assert.Equal(0, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1One2Manies.Add(c1d);

            Assert.Equal(0, c1a_C1C1One2Manies);
            Assert.Equal(0, c1b_C1WhereC1C1One2Many);
            Assert.Equal(0, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(2, workspaceChanged);

            /*  [given]               [when added]          [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a *   --* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Add(c1b);

            Assert.Equal(1, c1a_C1C1One2Manies);
            Assert.Equal(1, c1b_C1WhereC1C1One2Many);
            Assert.Equal(1, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(3, workspaceChanged);
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

            #region signals
            int c1a_C1C1One2Manies = 0;
            int c1b_C1WhereC1C1One2Many = 0;
            int c1c_C1C1One2Manies = 0;
            int c1d_C1WhereC1C1One2Many = 0;
            int workspaceChanged = 0;

            c1a.C1C1One2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1One2Manies;
            };

            c1b.C1WhereC1C1One2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1WhereC1C1One2Many;
            };

            c1c.C1C1One2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1One2Manies;
            };

            c1d.C1WhereC1C1One2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1WhereC1C1One2Many;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1One2Manies.Remove(c1c);

            Assert.Equal(0, c1a_C1C1One2Manies);
            Assert.Equal(0, c1b_C1WhereC1C1One2Many);
            Assert.Equal(0, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1One2Manies.Remove(c1a);

            Assert.Equal(0, c1a_C1C1One2Manies);
            Assert.Equal(0, c1b_C1WhereC1C1One2Many);
            Assert.Equal(0, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(2, workspaceChanged);

            /*  [given]               [when removed]        [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a -------* c1b
             *         -          -          -          =          
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1One2Manies.Remove(c1b);

            Assert.Equal(0, c1a_C1C1One2Manies);
            Assert.Equal(1, c1b_C1WhereC1C1One2Many);
            Assert.Equal(1, c1c_C1C1One2Manies);
            Assert.Equal(0, c1d_C1WhereC1C1One2Many);
            Assert.Equal(3, workspaceChanged);
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

            #region signals
            int c1a_C1C1Many2Manies = 0;
            int c1b_C1sWhereC1C1Many2Many = 0;
            int c1c_C1C1Many2Manies = 0;
            int c1d_C1sWhereC1C1Many2Many = 0;
            int workspaceChanged = 0;

            c1a.C1C1Many2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1Many2Manies;
            };

            c1b.C1sWhereC1C1Many2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1sWhereC1C1Many2Many;
            };

            c1c.C1C1Many2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1Many2Manies;
            };

            c1d.C1sWhereC1C1Many2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1sWhereC1C1Many2Many;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1Many2Manies.Add(c1b);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(0, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1Many2Manies.Add(c1d);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(0, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(2, workspaceChanged);

            /*  [given]               [when added]          [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *                    +          -          =          -
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1Many2Manies.Add(c1b);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(1, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(1, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(3, workspaceChanged);
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
            c1c.C1C1Many2Manies.Add(c1b);
            c1c.C1C1Many2Manies.Add(c1d);

            #region signals
            int c1a_C1C1Many2Manies = 0;
            int c1b_C1sWhereC1C1Many2Many = 0;
            int c1c_C1C1Many2Manies = 0;
            int c1d_C1sWhereC1C1Many2Many = 0;
            int workspaceChanged = 0;

            c1a.C1C1Many2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1a_C1C1Many2Manies;
            };

            c1b.C1sWhereC1C1Many2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1b_C1sWhereC1C1Many2Many;
            };

            c1c.C1C1Many2Manies.InvalidationRequested += (sender, args) =>
            {
                ++c1c_C1C1Many2Manies;
            };

            c1d.C1sWhereC1C1Many2Many.InvalidationRequested += (sender, args) =>
            {
                ++c1d_C1sWhereC1C1Many2Many;
            };

            workspace.InvalidationRequested += (sender, args) =>
            {
                ++workspaceChanged;
            };
            #endregion

            c1a.C1C1Many2Manies.Remove(c1c);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(0, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(1, workspaceChanged);

            c1c.C1C1Many2Manies.Remove(c1a);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(0, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(0, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(2, workspaceChanged);

            /*  [given]               [when removed]        [then changed]
             *
             *  c1a ------- c1b       c1a     --- c1b       c1a ------* c1b
             *         -          +          -          =           
             *  c1c ------- c1d       c1c ---     c1c       c1c *------ c1d
             *
             */

            c1c.C1C1Many2Manies.Remove(c1b);

            Assert.Equal(0, c1a_C1C1Many2Manies);
            Assert.Equal(1, c1b_C1sWhereC1C1Many2Many);
            Assert.Equal(1, c1c_C1C1Many2Manies);
            Assert.Equal(0, c1d_C1sWhereC1C1Many2Many);
            Assert.Equal(3, workspaceChanged);
        }

        [Fact]
        public async void Pull()
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

            int workspaceXChanges = 0;
            int workspaceYChanges = 0;

            #region signals

            workspaceX.InvalidationRequested += (sender, args) =>
            {
                ++workspaceXChanges;
            };

            workspaceY.InvalidationRequested += (sender, args) =>
            {
                ++workspaceYChanges;
            };
            #endregion
            
            var yResult = await workspaceY.PullAsync(pull);

            Assert.Equal(0, workspaceXChanges);
            Assert.Equal(1, workspaceYChanges);

            var yC1A = yResult.GetCollection<C1>().First();
            yC1A.C1AllorsString.Value = "New New New";

            Assert.Equal(0, workspaceXChanges);
            Assert.Equal(2, workspaceYChanges);

            await workspaceY.PushAsync();

            Assert.Equal(0, workspaceXChanges);
            Assert.Equal(2, workspaceYChanges);

            await workspaceX.PullAsync(pull);

            Assert.Equal(1, workspaceXChanges);
            Assert.Equal(2, workspaceYChanges);

            Assert.Equal("New New New", xC1A.C1AllorsString.Value);

            xC1A.C1AllorsString.Value = "New New New";

            Assert.Equal(2, workspaceXChanges);
            Assert.Equal(2, workspaceYChanges);

            await workspaceX.PushAsync();
            
            Assert.Equal(2, workspaceXChanges);
            Assert.Equal(2, workspaceYChanges);

            await workspaceY.PullAsync(pull);

            Assert.Equal(2, workspaceXChanges);
            Assert.Equal(3, workspaceYChanges);
        }
    }
}

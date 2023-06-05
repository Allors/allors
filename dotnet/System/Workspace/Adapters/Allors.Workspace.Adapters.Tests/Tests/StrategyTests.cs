﻿// <copyright file="Many2OneTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Threading.Tasks;
    using Allors.Workspace.Domain;
    using Xunit;

    public abstract class StrategyTests : Test
    {
        protected StrategyTests(Fixture fixture) : base(fixture)
        {

        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public void SetUnitRoleWrongObjectType()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            Assert.NotNull(c1);

            bool hasErrors;

            try
            {
                c1.Strategy.UnitRole(this.M.C1.C1AllorsInteger).Value = "Not an integer";
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

        [Fact]
        public void SetCompositeRoleWrongObjectType()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            var c2 = workspace.Create<C2>();
            Assert.NotNull(c1);
            Assert.NotNull(c2);

            bool hasErrors;

            try
            {
                c1.Strategy.CompositeRole(this.M.C1.C1C1One2One).Value = c2.Strategy;
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

        [Fact]
        public void SetCompositeRoleWrongRoleType()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            var c2 = workspace.Create<C2>();
            Assert.NotNull(c1);
            Assert.NotNull(c2);

            bool hasErrors;

            c1.Strategy.CompositesRole(this.M.C1.C1C2Many2Manies).Value = new[] { c2.Strategy };

            try
            {
                c1.Strategy.CompositeRole(this.M.C1.C1C2Many2Manies).Value = c2.Strategy;
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

        [Fact]
        public void AddCompositesRoleWrongObjectType()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            var c2 = workspace.Create<C2>();

            bool hasErrors;
            try
            {
                c1.Strategy.CompositesRole(this.M.C1.C1C1Many2Manies).Add(c2.Strategy);
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

        [Fact]
        public void AddCompositesRoleWrongRoleType()
        {
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            var c2 = workspace.Create<C2>();
            Assert.NotNull(c1);
            Assert.NotNull(c2);

            bool hasErrors;

            try
            {
                c1.Strategy.CompositesRole(this.M.C1.C1C2One2One).Add(c2.Strategy);
                hasErrors = false;
            }
            catch (Exception)
            {
                hasErrors = true;
            }

            Assert.True(hasErrors);
        }

    }
}

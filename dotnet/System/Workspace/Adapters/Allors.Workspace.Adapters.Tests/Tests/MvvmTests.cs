// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Tests
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Xunit;

    public abstract class MvvmTests : Test
    {
        protected MvvmTests(Fixture fixture) : base(fixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await this.Login("administrator");
        }

        [Fact]
        public async void Test()
        {
            var workspace = this.Profile.Workspace;


            workspace.Create<Person>();
        }
    }
}

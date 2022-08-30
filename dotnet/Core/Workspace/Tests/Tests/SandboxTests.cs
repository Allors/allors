// <copyright file="ServicesTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using Xunit;

    public abstract class SandboxTests : Test
    {
        protected SandboxTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async void Test()
        {
            await this.Login("administrator");

            foreach (var connection in this.Connections)
            {

            }

        }
    }
}

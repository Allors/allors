// <copyright file="Test.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Direct
{
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Xunit;

    public abstract class Test : IAsyncLifetime
    {
        protected Test(Fixture fixture)
        {
        }

        public IWorkspaceConnection Workspace => this.Profile.WorkspaceConnection;

        public M M => (M)this.Workspace.MetaPopulation;

        public abstract IProfile Profile { get; }

        public virtual async Task InitializeAsync() => await this.Profile.InitializeAsync();

        public virtual async Task DisposeAsync() => await this.Profile.DisposeAsync();

        protected async Task Login(string userName) => await this.Profile.Login(userName);
    }
}

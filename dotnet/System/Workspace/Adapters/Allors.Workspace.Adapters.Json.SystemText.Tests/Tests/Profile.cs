// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;
using Allors.Workspace.Meta.Static;

namespace Allors.Workspace.Adapters.Json.SystemText.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Meta;
    using Xunit;
    using Configuration = Allors.Workspace.Adapters.Json.Configuration;
    using Connection = SystemText.Connection;
    using IWorkspaceServices = Allors.Workspace.IWorkspaceServices;

    public class Profile : IProfile
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "TestAuthentication/Token";

        private readonly Func<IWorkspaceServices> servicesBuilder;
        private readonly IdGenerator idGenerator;
        private readonly Configuration configuration;

        private HttpClient httpClient;

        public Profile()
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Allors.Workspace.Domain.Person));
            this.servicesBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);

            this.configuration = new Configuration("Default", metaPopulation);
            this.idGenerator = new IdGenerator();
        }

        IWorkspace IProfile.Workspace => this.Workspace;

        public Connection Connection { get; private set; }

        public IWorkspace Workspace { get; private set; }

        public M M => ((IWorkspaceServices)this.Workspace.Services).Get<M>();

        public async Task InitializeAsync()
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(Url), Timeout = TimeSpan.FromMinutes(30) };
            var response = await this.httpClient.GetAsync(SetupUrl);
            Assert.True(response.IsSuccessStatusCode);

            this.Connection = new Connection(this.configuration, this.servicesBuilder, this.httpClient, this.idGenerator);
            this.Workspace = this.Connection.CreateWorkspace();

            await this.Login("administrator");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public IWorkspace CreateExclusiveWorkspace()
        {
            var database = new Connection(this.configuration, this.servicesBuilder, this.httpClient, this.idGenerator);
            return database.CreateWorkspace();
        }

        public IWorkspace CreateSharedWorkspace() => this.Connection.CreateWorkspace();

        public async Task Login(string user)
        {
            var uri = new Uri(LoginUrl, UriKind.Relative);
            var response = await this.Connection.Login(uri, user, null);
            Assert.True(response);
        }
    }
}

// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Json
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Adapters.Json.SystemText;
    using Allors.Workspace.Meta;
    using Xunit;
    using Configuration = Allors.Workspace.Adapters.Json.Configuration;
    using DatabaseConnection = Allors.Workspace.Adapters.Json.SystemText.DatabaseConnection;
    using IWorkspaceServices = Allors.Workspace.IWorkspaceServices;

    public class Profile : IProfile
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "TestAuthentication/Token";

        private readonly Func<IWorkspaceServices> servicesBuilder;
        private readonly Configuration configuration;

        private HttpClient httpClient;

        public Profile()
        {
            this.servicesBuilder = () => new WorkspaceServices();

            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Allors.Workspace.Domain.Person));
            this.configuration = new Configuration("Default", metaPopulation, objectFactory);
        }

        IWorkspaceConnection IProfile.Workspace => this.Workspace;

        public DatabaseConnection DatabaseConnection { get; private set; }

        public IWorkspaceConnection Workspace { get; private set; }

        public M M => this.Workspace.Services.Get<M>();

        public async Task InitializeAsync()
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(Url), Timeout = TimeSpan.FromMinutes(30) };
            var response = await this.httpClient.GetAsync(SetupUrl);
            Assert.True(response.IsSuccessStatusCode);

            this.DatabaseConnection = new DatabaseConnection(this.configuration, this.servicesBuilder, new Client(() => this.httpClient));
            this.Workspace = this.DatabaseConnection.CreateWorkspaceConnection();

            await this.Login("administrator");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public IWorkspaceConnection CreateExclusiveWorkspace()
        {
            var database = new DatabaseConnection(this.configuration, this.servicesBuilder, new Client(() => this.httpClient));
            return database.CreateWorkspaceConnection();
        }

        public IWorkspaceConnection CreateWorkspace() => this.DatabaseConnection.CreateWorkspaceConnection();

        public async Task Login(string user)
        {
            var uri = new Uri(LoginUrl, UriKind.Relative);
            var response = await this.DatabaseConnection.Client.Login(uri, user, null);
            Assert.True(response);
        }
    }
}

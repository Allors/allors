// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Adapters.Tests;
using Allors.Workspace.Meta.Static;

namespace Allors.Workspace.Adapters.Json.Newtonsoft.Tests
{
    using System;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Adapters.Json.Newtonsoft;
    using Allors.Workspace.Meta;
    using RestSharp;
    using RestSharp.Serializers.NewtonsoftJson;
    using Xunit;
    using Configuration = Allors.Workspace.Adapters.Json.Configuration;
    using Connection = Newtonsoft.Connection;

    public class Profile : IProfile
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "TestAuthentication/Token";

        private readonly Func<IWorkspaceServices> serviceBuilder;
        private readonly Configuration configuration;
        private readonly IdGenerator idGenerator;

        private Client client;

        IWorkspace IProfile.Workspace => this.Workspace;

        public Connection Connection { get; private set; }

        public IWorkspace Workspace { get; private set; }

        public M M => this.Workspace.Services.Get<M>();

        public Profile()
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Allors.Workspace.Domain.Person));
            this.serviceBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);

            this.configuration = new Configuration("Default", metaPopulation);
            this.idGenerator = new IdGenerator();
        }

        public async Task InitializeAsync()
        {
            var request = new RestRequest($"{Url}{SetupUrl}", RestSharp.Method.GET, DataFormat.Json);
            var restClient = this.CreateRestClient();
            var response = await restClient.ExecuteAsync(request);
            Assert.True(response.IsSuccessful);

            this.client = new Client(this.CreateRestClient);
            this.Connection = new Connection(this.configuration, this.serviceBuilder, this.client, this.idGenerator);
            this.Workspace = this.Connection.CreateWorkspace();

            await this.Login("administrator");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public IWorkspace CreateExclusiveWorkspace()
        {
            var database = new Connection(this.configuration, this.serviceBuilder, this.client, this.idGenerator);
            return database.CreateWorkspace();
        }

        public IWorkspace CreateSharedWorkspace() => this.Connection.CreateWorkspace();

        public async Task Login(string user)
        {
            var uri = new Uri(LoginUrl, UriKind.Relative);
            var response = await this.client.Login(uri, user, null);
            Assert.True(response);
        }

        private IRestClient CreateRestClient() => new RestClient(Url).UseNewtonsoftJson();
    }
}

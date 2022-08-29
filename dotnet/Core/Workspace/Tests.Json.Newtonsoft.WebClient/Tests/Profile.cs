// <copyright file="Profile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace.Json
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Adapters.Json.Newtonsoft.WebClient;
    using Allors.Workspace.Meta;
    using Polly;
    using RestSharp;
    using RestSharp.Serializers.NewtonsoftJson;
    using Xunit;

    public class Profile : IProfile
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "TestAuthentication/Token";

        private readonly M metaPopulation;
        private readonly ReflectionObjectFactory objectFactory;

        private Client client;

        IWorkspaceConnection IProfile.WorkspaceConnection => this.WorkspaceConnection;

        public Allors.Workspace.Adapters.Json.Newtonsoft.WebClient.WorkspaceConnection WorkspaceConnection { get; private set; }

        public M M => (M)this.WorkspaceConnection.MetaPopulation;

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
            .Handle<WebException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public Profile()
        {
            this.metaPopulation = new MetaBuilder().Build();
            this.objectFactory = new ReflectionObjectFactory(this.metaPopulation, typeof(Allors.Workspace.Domain.Person));
        }

        public async Task InitializeAsync()
        {
            var request = new RestRequest($"{Url}{SetupUrl}", RestSharp.Method.GET, DataFormat.Json);
            var restClient = this.CreateRestClient();
            var response = await this.Policy.ExecuteAsync(async () => await restClient.ExecuteAsync(request));
            Assert.True(response.IsSuccessful);

            this.client = new Client(this.CreateRestClient);
            this.WorkspaceConnection = new Allors.Workspace.Adapters.Json.Newtonsoft.WebClient.WorkspaceConnection(this.client, "Default", this.metaPopulation, this.objectFactory);

            await this.Login("administrator");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public IWorkspaceConnection CreateExclusiveWorkspaceConnection() => new Allors.Workspace.Adapters.Json.Newtonsoft.WebClient.WorkspaceConnection(this.client, "Default", this.metaPopulation, this.objectFactory);

        public IWorkspaceConnection CreateWorkspaceConnection() => new Allors.Workspace.Adapters.Json.Newtonsoft.WebClient.WorkspaceConnection(this.client, "Default", this.metaPopulation, this.objectFactory);

        public async Task Login(string user)
        {
            var uri = new Uri(LoginUrl, UriKind.Relative);
            var response = await this.client.Login(uri, user, null);
            Assert.True(response);
        }

        private IRestClient CreateRestClient() => new RestClient(Url).UseNewtonsoftJson();
    }
}

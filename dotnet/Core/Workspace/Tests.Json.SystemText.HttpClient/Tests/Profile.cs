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
    using Allors.Workspace.Adapters.Json.SystemText;
    using Allors.Workspace.Meta;
    using Polly;
    using Xunit;
    using Connection = Allors.Workspace.Adapters.Json.SystemText.Connection;

    public class Profile : IProfile
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "TestAuthentication/Token";

        private HttpClient httpClient;

        private Client client;

        public Profile()
        {
            this.M = new MetaBuilder().Build();
        }

        IConnection IProfile.Connection => this.Connection;

        public IConnection Connection { get; private set; }

        public M M { get; }

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public async Task InitializeAsync()
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(Url), Timeout = TimeSpan.FromMinutes(30) };
            var response = await this.Policy.ExecuteAsync(async () => await this.httpClient.GetAsync(SetupUrl));
            Assert.True(response.IsSuccessStatusCode);

            this.client = new Client(() => this.httpClient);

            this.Connection = new Connection(this.client, "Default", this.M);

            await this.Login("administrator");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public IConnection CreateExclusiveWorkspaceConnection() => new Connection(this.client, "Default", this.M);

        public IConnection CreateWorkspaceConnection() => new Connection(this.client, "Default", this.M);

        public async Task Login(string user)
        {
            var uri = new Uri(LoginUrl, UriKind.Relative);
            var response = await this.client.Login(uri, user, null);
            Assert.True(response);
        }
    }
}

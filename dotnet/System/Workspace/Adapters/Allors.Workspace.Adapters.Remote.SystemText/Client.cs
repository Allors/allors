// <copyright file="RemoteDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote.SystemText
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Protocol.Json.Api.Pull;
    using Allors.Protocol.Json.Api.Push;
    using Allors.Protocol.Json.Api.Security;
    using Allors.Protocol.Json.Api.Sync;
    using Allors.Protocol.Json.Auth;
    using Allors.Protocol.Json.SystemTextJson;
    using Ranges;
    using Polly;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
    public class Client
    {
        public int[] SecondsBeforeRetry { get; set; } = { 1, 2, 4, 8, 16 };

        public Client(Func<HttpClient> httpClientFactory) => this.HttpClient = httpClientFactory();

        public HttpClient HttpClient { get; private set; }

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
          .Handle<HttpRequestException>()
          .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public string UserId { get; private set; }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            var request = new AuthenticationTokenRequest { l = username, p = password };
            using var response = await this.PostAsJsonAsync(url, request);
            response.EnsureSuccessStatusCode();
            var authResult = await this.ReadAsAsync<AuthenticationTokenResponse>(response);
            if (!authResult.a)
            {
                return false;
            }

            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.t);
            this.UserId = authResult.u;

            return true;
        }

        public void Logoff()
        {
            this.HttpClient = null;
            this.UserId = null;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync(Uri uri, object args) =>
                await this.Policy.ExecuteAsync(
                    async () =>
                    {
                        // TODO: use SerializeToUtf8Bytes()
                        var json = JsonSerializer.Serialize(args);
                        return await this.HttpClient.PostAsync(
                            uri,
                            new StringContent(json, Encoding.UTF8, "application/json"));
                    });

        public async Task<T> ReadAsAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}

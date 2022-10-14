// <copyright file="RemoteDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json.Newtonsoft.WebClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Protocol.Json.Api.Pull;
    using Allors.Protocol.Json.Api.Push;
    using Allors.Protocol.Json.Api.Security;
    using Allors.Protocol.Json.Api.Sync;
    using Allors.Protocol.Json.Newtonsoft;
    using Allors.Workspace.Meta;

    [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
    public class Connection : Json.Connection
    {
        private readonly Client client;

        public Connection(Client client, string name, MetaPopulation metaPopulation) : base(name, metaPopulation)
        {
            this.client = client;
            this.UnitConvert = new UnitConvert();
        }

        public override IUnitConvert UnitConvert { get; }

        protected override string UserId => this.client.UserId;

        public override async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("sync", UriKind.Relative);
            return await this.client.Post<SyncResponse>(uri, syncRequest);
        }

        public override async Task<InvokeResponse> Invoke(InvokeRequest invokeRequest)
        {
            var uri = new Uri("invoke", UriKind.Relative);
            return await this.client.Post<InvokeResponse>(uri, invokeRequest);
        }

        public override async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("push", UriKind.Relative);
            // TODO: Retry for network errors, but not for server errors
            return await this.client.PostOnce<PushResponse>(uri, pushRequest);
        }

        public override async Task<PullResponse> Pull(object args, string name)
        {
            var uri = new Uri($"{name}/pull", UriKind.Relative);
            return await this.client.Post<PullResponse>(uri, args);
        }

        public override async Task<PullResponse> Pull(PullRequest pullRequest)
        {
            var uri = new Uri("pull", UriKind.Relative);
            return await this.client.Post<PullResponse>(uri, pullRequest);
        }

        public override async Task<AccessResponse> Access(AccessRequest accessRequest)
        {
            var uri = new Uri("access", UriKind.Relative);
            return await this.client.Post<AccessResponse>(uri, accessRequest);
        }

        public override async Task<PermissionResponse> Permission(PermissionRequest permissionRequest)
        {
            var uri = new Uri("permission", UriKind.Relative);
            return await this.client.Post<PermissionResponse>(uri, permissionRequest);
        }
    }
}

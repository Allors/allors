// <copyright file="RemoteSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Threading.Tasks;
    using Allors.Protocol.Json.Api.Pull;
    using Response;

    public class Workspace : Adapters.Workspace
    {
        internal Workspace(Connection workspace) : base(workspace) { }

        public new Connection Connection => (Connection)base.Connection;

        internal async Task<IPullResult> OnPull(PullResponse pullResponse)
        {
            var pullResult = new PullResult(this, pullResponse);

            if (pullResult.HasErrors)
            {
                return pullResult;
            }

            var syncRequest = this.Connection.OnPullResponse(pullResponse);
            if (syncRequest.o.Length > 0)
            {
                var syncResponse = await this.Connection.Sync(syncRequest);
                var accessRequest = this.Connection.OnSyncResponse(syncResponse);

                if (accessRequest != null)
                {
                    var accessResponse = await this.Connection.Access(accessRequest);
                    var permissionRequest = this.Connection.AccessResponse(accessResponse);
                    if (permissionRequest != null)
                    {
                        var permissionResponse = await this.Connection.Permission(permissionRequest);
                        this.Connection.PermissionResponse(permissionResponse);
                    }
                }
            }

            foreach (var v in pullResponse.p)
            {
                if (this.ObjectByWorkspaceId.TryGetValue(v.i, out var strategy))
                {
                    strategy.OnPulled(pullResult);
                }
                else
                {
                    var record = (Record)base.Connection.GetRecord(v.i);
                    var @object = new Object(this, record);
                    this.AddObject(@object);
                }
            }

            return pullResult;
        }
    }
}

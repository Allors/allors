// <copyright file="RemoteSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Protocol.Json.Api.Pull;
    using Data;
    using Protocol.Json;
    using InvokeOptions = Allors.Workspace.InvokeOptions;

    public class Workspace : Adapters.Workspace
    {
        internal Workspace(Connection workspace) : base(workspace) { }

        public new Connection Connection => (Connection)base.Connection;

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = (DatabaseRecord)base.Connection.GetRecord(id);
            var strategy = new Strategy(this, databaseRecord);
            this.AddStrategy(strategy);
        }

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
                if (this.StrategyByWorkspaceId.TryGetValue(v.i, out var strategy))
                {
                    strategy.DatabaseOriginState.OnPulled(pullResult);
                }
                else
                {
                    this.InstantiateDatabaseStrategy(v.i);
                }
            }

            return pullResult;
        }

        public override async Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null) => await this.InvokeAsync(new[] { method }, options);

        public override async Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null)
        {
            var invokeRequest = new InvokeRequest
            {
                l = methods.Select(v => new Invocation
                {
                    i = v.Object.Id,
                    v = ((Strategy)v.Object.Strategy).DatabaseOriginState.Version,
                    m = v.MethodType.Tag
                }).ToArray(),
                o = options != null
                    ? new Allors.Protocol.Json.Api.Invoke.InvokeOptions
                    {
                        c = options.ContinueOnError,
                        i = options.Isolated
                    }
                    : null
            };

            var invokeResponse = await this.Connection.Invoke(invokeRequest);
            return new InvokeResult(this, invokeResponse);
        }

        public override async Task<IPullResult> CallAsync(object args, string name)
        {
            var pullResponse = await this.Connection.Pull(args, name);
            return await this.OnPull(pullResponse);
        }

        public override async Task<IPullResult> PullAsync(params Pull[] pulls)
        {
            foreach (var pull in pulls)
            {
                if (pull.ObjectId < 0 || pull.Object?.Id < 0)
                {
                    throw new ArgumentException($"Id is not in the database");
                }
            }

            var pullRequest = new PullRequest { l = pulls.Select(v => v.ToJson(this.Connection.UnitConvert)).ToArray() };

            var pullResponse = await this.Connection.Pull(pullRequest);
            return await this.OnPull(pullResponse);
        }

        public override async Task<IPullResult> CallAsync(Procedure procedure, params Pull[] pull)
        {
            var pullRequest = new PullRequest
            {
                p = procedure.ToJson(this.Connection.UnitConvert),
                l = pull.Select(v => v.ToJson(this.Connection.UnitConvert)).ToArray()
            };

            var pullResponse = await this.Connection.Pull(pullRequest);
            return await this.OnPull(pullResponse);
        }
    }
}

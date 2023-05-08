// <copyright file="RemoteWorkspace.cs" company="Allors bvba">
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
    using Allors.Protocol.Json.Api.Push;
    using Data;
    using Meta;
    using Protocol.Json;
    using InvokeOptions = Allors.Workspace.InvokeOptions;

    public class Workspace : Adapters.Workspace
    {
        public Workspace(DatabaseConnection database, IWorkspaceServices services) : base(database, services) => this.Services.OnInit(this);

        public new DatabaseConnection DatabaseConnection => (DatabaseConnection)base.DatabaseConnection;

        public override T Create<T>(IClass @class)
        {
            var workspaceId = base.DatabaseConnection.NextId();
            var strategy = new Strategy(this, @class, workspaceId);
            this.AddStrategy(strategy);
            this.PushToDatabaseTracker.OnCreated(strategy);
            return (T)strategy.Object;
        }

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = (DatabaseRecord)this.DatabaseConnection.GetRecord(id);
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

            var syncRequest = this.DatabaseConnection.OnPullResponse(pullResponse);
            if (syncRequest.o.Length > 0)
            {
                var database = this.DatabaseConnection;
                var syncResponse = await database.Sync(syncRequest);
                var accessRequest = database.OnSyncResponse(syncResponse);

                if (accessRequest != null)
                {
                    var accessResponse = await database.Access(accessRequest);
                    var permissionRequest = database.AccessResponse(accessResponse);
                    if (permissionRequest != null)
                    {
                        var permissionResponse = await database.Permission(permissionRequest);
                        database.PermissionResponse(permissionResponse);
                    }
                }
            }

            foreach (var v in pullResponse.p)
            {
                if (this.StrategyByWorkspaceId.TryGetValue(v.i, out var strategy))
                {
                    strategy.State.OnPulled(pullResult);
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
                    v = ((Strategy)v.Object.Strategy).State.Version,
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

            var invokeResponse = await this.DatabaseConnection.Invoke(invokeRequest);
            return new InvokeResult(this, invokeResponse);
        }

        public override async Task<IPullResult> CallAsync(object args, string name)
        {
            var pullResponse = await this.DatabaseConnection.Pull(args, name);
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

            var pullRequest = new PullRequest { l = pulls.Select(v => v.ToJson(this.DatabaseConnection.UnitConvert)).ToArray() };

            var pullResponse = await this.DatabaseConnection.Pull(pullRequest);
            return await this.OnPull(pullResponse);
        }

        public override async Task<IPushResult> PushAsync()
        {
            var databaseTracker = this.PushToDatabaseTracker;

            var pushRequest = new PushRequest
            {
                n = databaseTracker.Created?.Select(v => ((State)v.State).PushNew()).ToArray(),
                o = databaseTracker.Changed?.Select(v => ((State)v.Strategy.State).PushExisting()).ToArray()
            };
            var pushResponse = await this.DatabaseConnection.Push(pushRequest);

            if (pushResponse.HasErrors)
            {
                return new PushResult(this, pushResponse);
            }


            if (pushResponse.n != null)
            {
                foreach (var pushResponseNewObject in pushResponse.n)
                {
                    var workspaceId = pushResponseNewObject.w;
                    var databaseId = pushResponseNewObject.d;
                    this.OnDatabasePushResponseNew(workspaceId, databaseId);
                }
            }

            databaseTracker.Created = null;
            databaseTracker.Changed = null;

            if (pushRequest.o != null)
            {
                foreach (var id in pushRequest.o.Select(v => v.d))
                {
                    var strategy = this.GetStrategy(id);
                    strategy.OnDatabasePushed();
                }
            }

            return new PushResult(this, pushResponse);
        }
    }
}

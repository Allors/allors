// <copyright file="RemoteWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System;
    using System.Collections.Generic;
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
        public Workspace(Connection database, IWorkspaceServices services) : base(database, services) => this.Services.OnInit(this);

        private new Connection Connection => (Connection)base.Connection;

        public override IStrategy Create(IClass @class)
        {
            var workspaceId = base.Connection.NextId();
            var strategy = new Strategy(this, @class, workspaceId);
            this.AddStrategy(strategy);
            this.PushToDatabaseTracker.OnCreated(strategy);
            return strategy;
        }

        public override async Task<IInvokeResult> InvokeAsync(IMethod method, InvokeOptions options = null) => await this.InvokeAsync(new[] { method }, options);

        public override async Task<IInvokeResult> InvokeAsync(IMethod[] methods, InvokeOptions options = null)
        {
            var invokeRequest = new InvokeRequest
            {
                l = methods.Select(v => new Invocation
                {
                    i = v.Object.Id,
                    v = ((Strategy)v.Object).Version,
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
            var pullResult = new PullResult(this, pullResponse);

            if (pullResult.HasErrors)
            {
                return pullResult;
            }

            var syncRequest = this.Connection.OnPullResponse(pullResponse);
            if (syncRequest.o.Length > 0)
            {
                var database = this.Connection;
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

            foreach (var p in pullResponse.p)
            {
                if (!this.StrategyById.TryGetValue(p.i, out var strategy))
                {
                    var databaseRecord = (Adapters.Record)this.Connection.GetRecord(p.i);
                    strategy = new Strategy(this, databaseRecord.Class, databaseRecord.Id);
                    this.AddStrategy(strategy);
                }
            }

            var classes = new HashSet<IClass>();

            foreach (var p in pullResponse.p)
            {
                if (this.StrategyById.TryGetValue(p.i, out var strategy))
                {
                    strategy.OnPulled(pullResult);
                    classes.Add(strategy.Class);
                }
            }

            var associationTypes = classes.SelectMany(v => v.AssociationTypes).Distinct();

            this.RegisterReactions(associationTypes);

            this.HandleReactions();

            return pullResult;
        }

        public override async Task<IPushResult> PushAsync()
        {
            var pushRequest = new PushRequest
            {
                n = this.PushToDatabaseTracker.Created?.Select(v => ((Strategy)v).PushNew()).ToArray(),
                o = this.PushToDatabaseTracker.Changed?.Select(v => ((Strategy)v).PushExisting()).Where(v => v.r != null).ToArray()
            };
            var pushResponse = await this.Connection.Push(pushRequest);

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

            this.PushToDatabaseTracker.Created = null;
            this.PushToDatabaseTracker.Changed = null;

            if (pushRequest.o != null)
            {
                foreach (var id in pushRequest.o.Select(v => v.d))
                {
                    var strategy = this.GetStrategy(id);
                    strategy.OnPushed();
                }
            }

            return new PushResult(this, pushResponse);
        }
    }
}

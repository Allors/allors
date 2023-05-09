// <copyright file="LocalWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;

namespace Allors.Workspace.Adapters.Direct
{
    using System.Threading.Tasks;
    using System;
    using Shared.Ranges;

    public class Workspace : Adapters.Workspace
    {
        public Workspace(Connection database, IWorkspaceServices services) : base(database, services)
        {
            this.Services.OnInit(this);
        }

        public new Connection Connection => (Connection)base.Connection;

        public long UserId => this.Connection.UserId;


        public override T Create<T>(IClass @class)
        {
            var workspaceId = this.Connection.NextId();
            var strategy = new Strategy(this, @class, workspaceId);
            this.AddStrategy(strategy);
            this.PushToDatabaseTracker.OnCreated(strategy);
            return (T)strategy.Object;
        }

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = this.Connection.GetRecord(id);
            var strategy = new Strategy(this, (Adapters.Record)databaseRecord);
            this.AddStrategy(strategy);
        }

        public override Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null) =>
               this.InvokeAsync(new[] { method }, options);

        public override Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null)
        {
            var result = new Invoke(this);
            result.Execute(methods, options);
            return Task.FromResult<IInvokeResult>(result);
        }

        public override Task<IPullResult> CallAsync(object args, string name)
        {
            var result = new Pull(this);

            result.Execute(args, name);

            return Task.FromResult<IPullResult>(result);
        }

        public override Task<IPullResult> PullAsync(params Data.Pull[] pulls)
        {
            foreach (var pull in pulls)
            {
                if (pull.ObjectId < 0 || pull.Object?.Id < 0)
                {
                    throw new ArgumentException($"Id is not in the database");
                }
            }

            var result = new Pull(this);
            result.Execute(pulls);

            this.OnPulled(result);

            return Task.FromResult<IPullResult>(result);
        }

        public override Task<IPushResult> PushAsync()
        {
            var result = new Push(this);

            result.Execute(this.PushToDatabaseTracker);

            if (result.HasErrors)
            {
                return Task.FromResult<IPushResult>(result);
            }

            this.PushToDatabaseTracker.Changed = null;

            if (result.ObjectByNewId?.Count > 0)
            {
                foreach (var kvp in result.ObjectByNewId)
                {
                    var workspaceId = kvp.Key;
                    var databaseId = kvp.Value.Id;

                    this.OnDatabasePushResponseNew(workspaceId, databaseId);
                }
            }

            this.PushToDatabaseTracker.Created = null;

            foreach (var @object in result.Objects)
            {
                var strategy = this.GetStrategy(@object.Id);
                strategy.OnPushed();
            }

            return Task.FromResult<IPushResult>(result);
        }

        internal void OnPulled(Pull pull)
        {
            var syncObjects = this.Connection.ObjectsToSync(pull);
            this.Connection.Sync(syncObjects, pull.AccessControl);

            foreach (var databaseObject in pull.DatabaseObjects)
            {
                if (this.StrategyByWorkspaceId.TryGetValue(databaseObject.Id, out var strategy))
                {
                    strategy._OnPulled(pull);
                }
                else
                {
                    this.InstantiateDatabaseStrategy(databaseObject.Id);
                }
            }
        }


    }
}

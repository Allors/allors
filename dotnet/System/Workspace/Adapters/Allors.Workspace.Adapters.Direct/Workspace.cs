// <copyright file="LocalSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using System;
    using System.Threading.Tasks;

    public class Workspace : Adapters.Workspace
    {
        internal Workspace(Adapters.Connection connection) : base(connection)
        {
        }

        public new Connection Connection => (Connection)base.Connection;

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = this.Connection.GetRecord(id);
            var strategy = new Strategy(this, (DatabaseRecord)databaseRecord);
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

        public override Task<IPullResult> CallAsync(Data.Procedure procedure, params Data.Pull[] pull)
        {
            var result = new Pull(this);

            result.Execute(procedure);
            result.Execute(pull);

            this.OnPulled(result);

            return Task.FromResult<IPullResult>(result);
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

        internal void OnPulled(Pull pull)
        {
            var syncObjects = this.Connection.ObjectsToSync(pull);
            this.Connection.Sync(syncObjects, pull.AccessControl);

            foreach (var databaseObject in pull.DatabaseObjects)
            {
                if (this.StrategyByWorkspaceId.TryGetValue(databaseObject.Id, out var strategy))
                {
                    strategy.DatabaseOriginState.OnPulled(pull);
                }
                else
                {
                    this.InstantiateDatabaseStrategy(databaseObject.Id);
                }
            }
        }
    }
}

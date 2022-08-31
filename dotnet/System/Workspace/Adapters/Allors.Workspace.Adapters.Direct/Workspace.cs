// <copyright file="LocalSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    public class Workspace : Adapters.Workspace
    {
        internal Workspace(Adapters.Connection connection) : base(connection)
        {
        }

        public new Connection Connection => (Connection)base.Connection;

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = this.Connection.GetRecord(id);
            var strategy = new Object(this, (Record)databaseRecord);
            this.AddObject(strategy);
        }

        internal void OnPulled(Pull pull)
        {
            var syncObjects = this.Connection.ObjectsToSync(pull);
            this.Connection.Sync(syncObjects, pull.AccessControl);

            foreach (var databaseObject in pull.DatabaseObjects)
            {
                if (this.ObjectByWorkspaceId.TryGetValue(databaseObject.Id, out var strategy))
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

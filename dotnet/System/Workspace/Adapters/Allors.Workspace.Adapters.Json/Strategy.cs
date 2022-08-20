// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using Meta;

    public sealed class Strategy : Adapters.Strategy
    {
        internal Strategy(Adapters.Workspace workspace, Class @class, long id) : base(workspace, @class, id) => this.DatabaseOriginState = new DatabaseOriginState(this, (DatabaseRecord)((DatabaseConnection)workspace.WorkspaceConnection.DatabaseConnection).GetRecord(this.Id));

        internal Strategy(Workspace workspace, DatabaseRecord databaseRecord) : base(workspace, databaseRecord) => this.DatabaseOriginState = new DatabaseOriginState(this, databaseRecord);

        public new Workspace Session => (Workspace)base.Workspace;
    }
}

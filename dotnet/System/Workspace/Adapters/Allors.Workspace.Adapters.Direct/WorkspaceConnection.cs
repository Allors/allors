// <copyright file="LocalWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    public class WorkspaceConnection : Adapters.WorkspaceConnection
    {
        public WorkspaceConnection(DatabaseConnection database, IWorkspaceServices services) : base(database, services) => this.Services.OnInit(this);

        public new DatabaseConnection DatabaseConnection => (DatabaseConnection)base.DatabaseConnection;

        public long UserId => this.DatabaseConnection.UserId;

        public override IWorkspace CreateWorkspace() => new Workspace(this, this.Services);
    }
}

// <copyright file="RemoteWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    public class WorkspaceConnection : Adapters.WorkspaceConnection
    {
        public WorkspaceConnection(DatabaseConnection database, IWorkspaceServices services) : base(database, services) => this.Services.OnInit(this);

        public new DatabaseConnection DatabaseConnection => (DatabaseConnection)base.DatabaseConnection;

        public override IWorkspace CreateWorkspace() => new Workspace(this, this.Services);
    }
}

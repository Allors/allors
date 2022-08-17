// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public abstract class Workspace : IWorkspace
    {
        protected Workspace(DatabaseConnection database, IWorkspaceServices services)
        {
            this.DatabaseConnection = database;
            this.Services = services;
        }

        public DatabaseConnection DatabaseConnection { get; }

        public IConfiguration Configuration => this.DatabaseConnection.Configuration;

        public IWorkspaceServices Services { get; }

        public abstract ISession CreateSession();
    }
}

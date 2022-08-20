// <copyright file="v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public abstract class DatabaseConnection
    {
        protected DatabaseConnection(Configuration configuration) => this.Configuration = configuration;

        public Configuration Configuration { get; }

        public abstract IWorkspaceConnection CreateWorkspaceConnection();

        public abstract DatabaseRecord GetRecord(long id);

        public abstract long GetPermission(Class @class, IOperandType operandType, Operations operation);
    }
}

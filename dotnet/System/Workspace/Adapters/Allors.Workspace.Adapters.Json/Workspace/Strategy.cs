// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using Meta;

    public sealed class Strategy : Adapters.Strategy
    {
        internal Strategy(Adapters.Workspace workspace, IClass @class, long id) : base(workspace, @class, id) => this.DatabaseState = new DatabaseState(this, (DatabaseRecord)((DatabaseConnection)workspace.DatabaseConnection).GetRecord(this.Id));

        internal Strategy(Workspace workspace, DatabaseRecord databaseRecord) : base(workspace, databaseRecord) => this.DatabaseState = new DatabaseState(this, databaseRecord);

        public new Workspace Session => (Workspace)base.Workspace;
    }
}

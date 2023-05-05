// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using Meta;

    public sealed class Strategy : Adapters.Strategy
    {
        internal Strategy(Adapters.Workspace session, IClass @class, long id) : base(session, @class, id) => this.DatabaseState = new DatabaseState(this, (DatabaseRecord)((DatabaseConnection)session.DatabaseConnection).GetRecord(this.Id));

        internal Strategy(Workspace session, DatabaseRecord databaseRecord) : base(session, databaseRecord) => this.DatabaseState = new DatabaseState(this, databaseRecord);

        public new Workspace Session => (Workspace)base.Session;
    }
}

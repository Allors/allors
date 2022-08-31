// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using Meta;

    public sealed class Object : Adapters.Object
    {
        internal Object(Adapters.Workspace workspace, Class @class, long id) : base(workspace, @class, id) => this.DatabaseOriginState = new DatabaseOriginState(this, workspace.Connection.GetRecord(this.Id));

        internal Object(Adapters.Workspace workspace, Adapters.Record record) : base(workspace, record) => this.DatabaseOriginState = new DatabaseOriginState(this, record);
    }
}

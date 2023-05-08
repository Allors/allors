﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using Meta;

    public sealed class Strategy : Adapters.Strategy
    {
        internal Strategy(Adapters.Workspace workspace, IClass @class, long id) : base(workspace, @class, id) => this.State = new State(this, workspace.Connection.GetRecord(this.Id));

        internal Strategy(Adapters.Workspace workspace, Adapters.Record record) : base(workspace, record) => this.State = new State(this, record);
    }
}

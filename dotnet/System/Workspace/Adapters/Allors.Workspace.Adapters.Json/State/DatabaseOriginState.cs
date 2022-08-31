// <copyright file="DatabaseOriginState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    internal sealed class DatabaseOriginState : Adapters.RecordBasedOriginState
    {
        internal DatabaseOriginState(Object @object, Record record) : base(record) => this.RemoteObject = @object;

        public override Adapters.Object Object => this.RemoteObject;
        private Object RemoteObject { get; }
    }
}

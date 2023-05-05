// <copyright file="DatabaseState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    internal sealed class DatabaseState : Adapters.DatabaseState
    {
        internal DatabaseState(Strategy strategy, Adapters.DatabaseRecord record) : base(record) => this.Strategy = strategy;

        public override Adapters.Strategy Strategy { get; }
    }
}

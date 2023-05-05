// <copyright file="ChangeSetTracker.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;

    public sealed class ChangeSetTracker
    {
        public ChangeSetTracker(Workspace session) => this.Session = session;

        public Workspace Session { get; set; }

        public ISet<IStrategy> Created { get; set; }

        public ISet<IStrategy> Instantiated { get; set; }

        public ISet<DatabaseState> DatabaseOriginStates { get; set; }

        public void OnCreated(Strategy strategy)
        {
            (this.Created ??= new HashSet<IStrategy>()).Add(strategy);
            this.Session.OnChanged(EventArgs.Empty);
        }

        public void OnInstantiated(Strategy strategy)
        {
            (this.Instantiated ??= new HashSet<IStrategy>()).Add(strategy);
            this.Session.OnChanged(EventArgs.Empty);
        }

        public void OnDatabaseChanged(DatabaseState state)
        {
            (this.DatabaseOriginStates ??= new HashSet<DatabaseState>()).Add(state);
            this.Session.OnChanged(EventArgs.Empty);
        }
    }
}

// <copyright file="PushToDatabaseTracker.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;

    public sealed class PushToDatabaseTracker
    {
        public ISet<Strategy> Created { get; set; }

        public ISet<Strategy> Changed { get; set; }

        public void OnCreated(Strategy strategy) => (this.Created ??= new HashSet<Strategy>()).Add(strategy);

        public void OnChanged(Strategy state)
        {
            if (!state.IsNew)
            {
                (this.Changed ??= new HashSet<Strategy>()).Add(state);
            }
        }
    }
}

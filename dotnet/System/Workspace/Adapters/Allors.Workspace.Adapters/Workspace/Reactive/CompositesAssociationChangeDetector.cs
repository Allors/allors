// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;

    public class CompositesAssociationChangeDetector : IChangeDetector
    {
        private readonly IAssociation association;
        private HashSet<IStrategy> value;

        public CompositesAssociationChangeDetector(IAssociation association)
        {
            this.association = association;
            this.value = [.. (IEnumerable<IStrategy>)this.association.Value];
        }

        public event ChangedEventHandler Changed;

        public bool HasHandlers => this.Changed?.GetInvocationList().Length > 0;

        public void Handle()
        {
            bool hasChanges = !this.value.SetEquals((IEnumerable<IStrategy>)this.association.Value);

            if (!hasChanges)
            {
                return;
            }

            this.value = [.. (IEnumerable<IStrategy>)this.association.Value];

            var changed = this.Changed;
            changed?.Invoke(this.association, EventArgs.Empty);
        }
    }
}

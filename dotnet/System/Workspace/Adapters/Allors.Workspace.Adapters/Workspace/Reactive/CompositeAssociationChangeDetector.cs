﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public class CompositeAssociationChangeDetector : IChangeDetector
    {
        private readonly IAssociation association;
        private IStrategy value;

        private InvalidationRequestedEventArgs invalidationRequestedEventArgs;

        public CompositeAssociationChangeDetector(IAssociation association)
        {
            this.association = association;
            this.value = (IStrategy)this.association.Value;
        }

        public event InvalidationRequestedEventHandler Changed;

        public bool HasHandlers => this.Changed?.GetInvocationList().Length > 0;

        public void Handle()
        {
            bool hasChanges = !Equals(this.value, this.association.Value);

            if (!hasChanges)
            {
                return;
            }

            this.value = (IStrategy)this.association.Value;

            var handler = this.Changed;
            handler?.Invoke(this.association, this.invalidationRequestedEventArgs ??= new InvalidationRequestedEventArgs(this.association));
        }
    }
}

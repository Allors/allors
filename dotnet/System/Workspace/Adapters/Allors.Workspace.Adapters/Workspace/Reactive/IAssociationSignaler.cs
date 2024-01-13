// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class AssociationSignaler : ISignaler
    {
        private readonly IAssociation association;
        private object value;

        public AssociationSignaler(IAssociation association)
        {
            this.association = association;
            this.value = this.association.Value;
        }

        public event ChangedEventHandler Changed;

        public bool HasHandlers => this.Changed?.GetInvocationList().Length > 0;

        public void Handle()
        {
            bool hasChanges;

            if (this.association.AssociationType.IsMany)
            {
                hasChanges = Equals(this.value, this.association.Value);
            }
            else
            {
                hasChanges = Equals(this.value, this.association.Value);
            }

            if (!hasChanges)
            {
                return;
            }

            this.value = this.association.Value;

            var changed = this.Changed;
            changed?.Invoke(this.association, EventArgs.Empty);
        }
    }
}

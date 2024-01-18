// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class UnitRoleChangeDetector : IChangeDetector
    {
        private readonly IRole role;
        private bool canRead;
        private bool canWrite;
        private object value;

        private InvalidationRequestedEventArgs invalidationRequestedEventArgs;

        public UnitRoleChangeDetector(IRole role)
        {
            this.role = role;
            this.canRead = this.role.CanRead;
            this.canWrite = this.role.CanWrite;
            this.value = this.role.Value;
        }

        public event InvalidationRequestedEventHandler Changed;

        public bool HasHandlers => this.Changed?.GetInvocationList().Length > 0;

        public void Handle()
        {
            var hasChanges = this.canRead != this.role.CanRead || this.canWrite != this.role.CanWrite;

            if (!hasChanges)
            {
                hasChanges = !Equals(this.value, this.role.Value);
            }

            if (!hasChanges)
            {
                return;
            }

            this.canRead = this.role.CanRead;
            this.canWrite = this.role.CanWrite;
            this.value = this.role.Value;

            var changed = this.Changed;
            changed?.Invoke(this.role, this.invalidationRequestedEventArgs ??= new InvalidationRequestedEventArgs(this.role));
        }
    }
}

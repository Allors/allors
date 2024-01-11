﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class CompositeRoleSignaler : ISignaler
    {
        private readonly IRoleInternal role;
        private bool canRead;
        private bool canWrite;
        private IStrategy value;

        public CompositeRoleSignaler(IRoleInternal role)
        {
            this.role = role;
            this.canRead = this.role.CanRead;
            this.canWrite = this.role.CanWrite;
            this.value = (IStrategy)this.role.Value;
        }

        public event ChangedEventHandler Changed;

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

            this.role.BumpVersion();

            this.canRead = this.role.CanRead;
            this.canWrite = this.role.CanWrite;
            this.value = (IStrategy)this.role.Value;

            var changed = this.Changed;
            changed?.Invoke(this.role, EventArgs.Empty);
        }
    }
}

// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class MethodChangeDetector : IChangeDetector
    {
        private readonly Method method;
        private bool canExecute;

        private ChangedEventArgs changedEventArgs;

        public MethodChangeDetector(Method method)
        {
            this.method = method;
            this.canExecute = this.method.CanExecute;
        }

        public event ChangedEventHandler Changed;

        public bool HasHandlers => this.Changed?.GetInvocationList().Length > 0;

        public void Handle()
        {
            if (this.canExecute == this.method.CanExecute)
            {
                return;
            }

            this.canExecute = this.method.CanExecute;
            
            var changed = this.Changed;
            changed?.Invoke(this.method, this.changedEventArgs ??= new ChangedEventArgs(this.method));
        }
    }
}

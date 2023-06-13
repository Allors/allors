// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.ComponentModel;
    using System.Linq;
    using Adapters;

    public class MethodReaction : IReaction
    {
        private bool canExecute;

        public MethodReaction(Method role)
        {
            this.Role = role;
            this.TakeSnapshot();
        }

        public Method Role { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasEventHandlers => this.PropertyChanged?.GetInvocationList().Any() == true;

        public void React()
        {
            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                if (!Equals(this.Role.CanExecute, this.canExecute))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("CanExecute"));
                }
            }

            this.TakeSnapshot();
        }

        private void TakeSnapshot()
        {
            this.canExecute = this.Role.CanExecute;
        }
    }
}

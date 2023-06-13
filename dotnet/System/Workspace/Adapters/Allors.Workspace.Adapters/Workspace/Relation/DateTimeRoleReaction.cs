// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Adapters;

    public class DateTimeRoleReaction : IReaction
    {
        private DateTime? value;
        private bool exist;
        private bool canRead;
        private bool canWrite;
        private bool isModified;

        public DateTimeRoleReaction(DateTimeRole role)
        {
            this.Role = role;
            this.TakeSnapshot();
        }

        public DateTimeRole Role { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasEventHandlers => this.PropertyChanged?.GetInvocationList().Any() == true;
        
        public void React()
        {
            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                if (!Equals(this.Role.Value, this.value))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("Value"));
                }

                if (!Equals(this.Role.Exist, this.exist))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("Exist"));
                }

                if (!Equals(this.Role.CanRead, this.canRead))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("CanRead"));
                }

                if (!Equals(this.Role.CanWrite, this.canWrite))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("CanWrite"));
                }

                if (!Equals(this.Role.IsModified, this.isModified))
                {
                    propertyChanged(this.Role, new PropertyChangedEventArgs("IsModified"));
                }
            }

            this.TakeSnapshot();
        }

        private void TakeSnapshot()
        {
            this.value = this.Role.Value;
            this.exist = this.Role.Exist;
            this.canRead = this.Role.CanRead;
            this.canWrite = this.Role.CanWrite;
            this.isModified = this.Role.IsModified;
        }
    }
}

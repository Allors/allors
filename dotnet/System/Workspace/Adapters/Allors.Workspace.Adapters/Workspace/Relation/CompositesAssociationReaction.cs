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

    public class CompositesAssociationReaction : IReaction
    {
        private IStrategy[] value;

        public CompositesAssociationReaction(CompositesAssociation association)
        {
            this.Association = association;
            this.TakeSnapshot();
        }

        public CompositesAssociation Association { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasEventHandlers => this.PropertyChanged?.GetInvocationList().Any() == true;

        public void React()
        {
            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                //TODO: Should be RefRanges
                if (!Equals(this.Association.Value, this.value))
                {
                    propertyChanged(this.Association, new PropertyChangedEventArgs("Value"));
                }
            }

            this.TakeSnapshot();
        }

        private void TakeSnapshot()
        {
            this.value = this.Association.Value?.ToArray() ?? Array.Empty<IStrategy>();
        }
    }
}

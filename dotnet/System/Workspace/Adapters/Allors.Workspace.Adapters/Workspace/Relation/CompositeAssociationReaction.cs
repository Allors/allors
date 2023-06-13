// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.ComponentModel;
    using System.Linq;
    using Adapters;

    public class CompositeAssociationReaction : IReaction
    {
        private IStrategy value;

        public CompositeAssociationReaction(CompositeAssociation association)
        {
            this.Association = association;
            this.TakeSnapshot();
        }

        public CompositeAssociation Association { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasEventHandlers => this.PropertyChanged?.GetInvocationList().Any() == true;
        
        public void React()
        {
            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                if (!Equals(this.Association.Value, this.value))
                {
                    propertyChanged(this.Association, new PropertyChangedEventArgs("Value"));
                }
            }

            this.TakeSnapshot();
        }

        private void TakeSnapshot()
        {
            this.value = this.Association.Value;
        }
    }
}

// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.ComponentModel;
    using Adapters;
    using Meta;

    public class CompositeAssociation : ICompositeAssociation, IAssociationInternals
    {
        private readonly Object lockObject = new();

        public CompositeAssociation(Strategy @object, IAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.AssociationType.RelationType;

        public IAssociationType AssociationType { get; }

        object IRelationEnd.Value => this.Value;

        public IStrategy Value => this.Object.GetCompositeAssociation(this.AssociationType);

        IReaction IReactiveInternals.Reaction => this.Reaction;

        public CompositeAssociationReaction Reaction { get; private set; }

        public override string ToString()
        {
            return $"[{Value?.Id}]";
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (this.lockObject)
                {
                    if (this.Reaction == null)
                    {
                        this.Reaction = new CompositeAssociationReaction(this);
                        //this.Reaction.Register();
                    }

                    this.Reaction.PropertyChanged += value;
                }
            }

            remove
            {
                lock (this.lockObject)
                {
                    this.Reaction.PropertyChanged -= value;

                    if (!this.Reaction.HasEventHandlers)
                    {
                        //this.Reaction.Deregister();
                        this.Reaction = null;
                    }
                }
            }
        }
    }
}

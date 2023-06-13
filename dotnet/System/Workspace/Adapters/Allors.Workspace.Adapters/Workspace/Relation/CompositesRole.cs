// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Adapters;
    using Meta;

    public class CompositesRole : ICompositesRole, IRoleInternals
    {
        private readonly Object lockObject = new();

        public CompositesRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }


        public IRelationType RelationType => this.RoleType.RelationType;


        public IRoleType RoleType { get; }

        object IRelationEnd.Value => this.Value;

        void ICompositesRole.Add(IStrategy value)
        {
            this.Object.AddCompositesRole(this.RoleType, value);
        }

        void ICompositesRole.Remove(IStrategy value)
        {
            this.Object.RemoveCompositesRole(this.RoleType, value);
        }

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (IEnumerable<IStrategy>)value;
        }
        
        public IEnumerable<IStrategy> Value
        {
            get => this.Object.GetCompositesRole(this.RoleType);
            set => this.Object.SetCompositesRole(this.RoleType, value);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);

        IReaction IReactiveInternals.Reaction => this.Reaction;

        public CompositesRoleReaction Reaction { get; private set; }

        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (this.lockObject)
                {
                    if (this.Reaction == null)
                    {
                        this.Reaction = new CompositesRoleReaction(this);
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

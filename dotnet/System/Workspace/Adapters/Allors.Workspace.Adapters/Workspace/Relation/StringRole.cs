﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.ComponentModel;
    using Adapters;
    using Meta;

    public class StringRole : IUnitRole<string>, IRoleInternals
    {
        private readonly Object lockObject = new();

        public StringRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType { get; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (string)value;
        }

        public string Value
        {
            get => (string)this.Object.GetUnitRole(this.RoleType);
            set => this.Object.SetUnitRole(this.RoleType, value);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);

        IReaction IReactiveInternals.Reaction => this.Reaction;

        public StringRoleReaction Reaction { get; private set; }

        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (this.lockObject)
                {
                    if (this.Reaction == null)
                    {
                        this.Reaction = new StringRoleReaction(this);
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

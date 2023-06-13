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

    public class Method : IMethod, IMethodInternals
    {
        private readonly Object lockObject = new();

        public Method(Strategy strategy, IMethodType methodType)
        {
            this.Object = strategy;
            this.MethodType = methodType;
        }

        IStrategy IMethod.Object => this.Object;

        public Strategy Object { get; }

        public IMethodType MethodType { get; }

        public bool CanExecute => this.Object.CanExecute(this.MethodType);

        IReaction IReactiveInternals.Reaction => this.Reaction;

        public MethodReaction Reaction { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (this.lockObject)
                {
                    if (this.Reaction == null)
                    {
                        this.Reaction = new MethodReaction(this);
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

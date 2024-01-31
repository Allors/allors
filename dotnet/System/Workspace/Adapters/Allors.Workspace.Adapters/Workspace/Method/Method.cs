﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Adapters;
    using Meta;

    public class Method : IMethod
    {
        public Method(Strategy strategy, IMethodType methodType)
        {
            this.Object = strategy;
            this.MethodType = methodType;
        }

        IStrategy IMethod.Object => this.Object;

        public Strategy Object { get; }

        public IMethodType MethodType { get; }

        public bool CanExecute => this.Object.CanExecute(this.MethodType);

        #region Reactive
        public IDisposable Subscribe(IObserver<IObserved> observer)
        {
            return this.Object.Workspace.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<IOperand> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }

        public IDisposable Subscribe(IObserver<IMethod> observer)
        {
            return this.Subscribe((IObserver<IObserved>)observer);
        }
        #endregion
    }
}

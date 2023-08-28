// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals.Default
{
    using System;
    using System.Collections.Generic;

    public class Dispatcher : IDispatcher
    {
        public IValueSignal<T> CreateValueSignal<T>(T value)
        {
            return new ValueSignal<T>(value);
        }

        public IComputedSignal<T> CreateCalculatedSignal<T>(Func<IDependencyTracker, T> calculation)
        {
            return new ComputedSignal<T>(calculation);
        }

        public IEffect CreateEffect(Action<IDependencyTracker> dependencies, Action action)
        {
            return new Effect(dependencies, action);
        }
        
        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }
    }
}

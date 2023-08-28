// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public interface IDispatcher
    {
        IValueSignal<T> CreateValueSignal<T>(T value);

        ICalculatedSignal<T> CreateCalculatedSignal<T>(Func<IDependencyTracker, T> calculation);

        IEffect CreateEffect<T>(T context, Action<T, IDependencyTracker> dependencies, Action<T> action);

        void Pause();

        void Resume();
    }
}

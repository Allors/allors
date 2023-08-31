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

        IComputedSignal<T> CreateCalculatedSignal<T>(Func<IDependency, T> calculation);

        IEffect CreateEffect(Action<IDependency> dependencies, Action action);

        void Pause();

        void Resume();

        void Schedule();
    }
}

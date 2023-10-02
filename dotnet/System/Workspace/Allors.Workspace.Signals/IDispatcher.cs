﻿// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    using System;

    public interface IDispatcher : IDisposable
    {
        IValueSignal<T> CreateValueSignal<T>(T value);

        IComputedSignal<T> CreateComputedSignal<T>(Func<ITracker, T> calculation);

        IEffect CreateEffect(Action<ITracker> dependencies, Action action);

        void Pause();

        void Resume();
    }
}

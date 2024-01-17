﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    using System;

    public interface ISignal : INotifyChanged
    {
        long Version { get; }

        object Value { get; }
    }

    public interface ISignal<out T> : ISignal
    {
        new T Value { get; }
    }
}

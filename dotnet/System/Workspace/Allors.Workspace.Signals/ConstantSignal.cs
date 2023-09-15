﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    public class ConstantSignal<T> : IComputedSignal<T>
    {
        public ConstantSignal(T value)
        {
            this.Value = value;
        }

        public long WorkspaceVersion => 0;

        object ISignal.Value => this.Value;

        public T Value { get; }
    }
}
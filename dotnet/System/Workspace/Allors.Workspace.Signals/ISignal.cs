// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    public interface ISignal : IOperand
    {
        object Value { get; }
    }

    public interface ISignal<out T> : ISignal
    {
        new T Value { get; }
    }
}

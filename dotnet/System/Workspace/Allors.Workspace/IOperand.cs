// <copyright file="IOperand.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Signals;

    public interface IOperand : ISignal
    {
    }

    public interface IOperand<out T> : IOperand, ISignal<T>
    {
    }
}

// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface IValueSignal : ISignal
    {
        new object Value { get; set; }
    }

    public interface IValueSignal<T> : ISignal<T>, IValueSignal
    {
        new T Value { get; set; }
    }
}

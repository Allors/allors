// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    using System;
    using System.Collections.Generic;

    public interface IEffect : IDisposable
    {
        Action<IChangedEventSource> Action { get; }

        void Add(INotifyChanged changeNotifier);

        void Remove(INotifyChanged changeNotifier);

        IEnumerable<INotifyChanged> ChangeNotifiers { get; }
    }
}

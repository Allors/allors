// <copyright file="ChangedEventHandler.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class ChangedEventArgs(IChangedEventSource source) : EventArgs
    {
        public IChangedEventSource Source { get; } = source;
    }
}

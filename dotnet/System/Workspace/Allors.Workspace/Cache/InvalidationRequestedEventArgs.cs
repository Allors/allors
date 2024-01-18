// <copyright file="InvalidationRequestedEventArgs.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class InvalidationRequestedEventArgs(ICacheable cacheable) : EventArgs
    {
        public ICacheable Cacheable { get; } = cacheable;
    }
}

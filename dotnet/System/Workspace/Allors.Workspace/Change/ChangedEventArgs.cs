// <copyright file="ChangedEventArgs.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class ChangedEventArgs(IChangeable changeable) : EventArgs
    {
        public IChangeable Changeable { get; } = changeable;
    }
}

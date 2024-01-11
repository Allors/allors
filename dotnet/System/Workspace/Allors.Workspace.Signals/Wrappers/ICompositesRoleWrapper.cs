﻿// <copyright file="ITime.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    using System.Collections.Generic;

    public interface ICompositesRoleWrapper<T>
    {
        IEnumerable<T> Value { get; set; }
    }
}

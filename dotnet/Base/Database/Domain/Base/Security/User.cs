﻿// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Security;

    public partial interface User : IUser
    {
        internal static string Normalize(string value) => value?.Normalize().ToUpperInvariant();
    }
}

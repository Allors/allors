// <copyright file="Validation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

public abstract class Validation
{
    public abstract string Message { get; }

    public abstract bool IsValid { get; }
}

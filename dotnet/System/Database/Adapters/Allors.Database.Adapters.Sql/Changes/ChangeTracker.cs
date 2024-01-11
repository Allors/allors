// <copyright file="ChangeSet.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Database.Adapters.Sql;

using Allors.Shared.Ranges;

internal struct ChangeTracker
{
    internal ValueRange<long> Add { get; set; }

    internal ValueRange<long> Remove { get; set; }
}

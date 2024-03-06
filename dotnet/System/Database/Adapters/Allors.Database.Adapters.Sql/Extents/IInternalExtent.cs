// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Meta;

public interface IInternalExtent
{
    IInternalExtent InExtent { get; }

    ExtentSort Sorter { get; }

    Transaction Transaction { get; }

    Composite ObjectType { get; }

    IInternalExtent ParentOperationExtent { get; set; }

    string BuildSql(ExtentStatement inStatement);

    void FlushCache();
}

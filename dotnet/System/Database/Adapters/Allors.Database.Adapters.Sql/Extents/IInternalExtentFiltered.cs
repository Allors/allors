// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Meta;

public interface IInternalExtentFiltered
{
    void FlushCache();

    void CheckRole(RoleType role);

    void CheckAssociation(AssociationType association);

    Strategy Strategy { get; }

    AssociationType AssociationType { get; }

    RoleType RoleType { get; }

    ICompositePredicate Filter { get; }

    Composite ObjectType { get; }
}

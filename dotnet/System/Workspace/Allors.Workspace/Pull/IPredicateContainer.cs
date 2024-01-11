// <copyright file="IPredicateContainer.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    public interface IPredicateContainer
    {
        void AddPredicate(IPredicate predicate);
    }
}

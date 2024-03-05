// <copyright file="ExtentT.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ExtentT type.</summary>

namespace Allors.Database;

using System.Collections.Generic;
using Allors.Database.Meta;

/// <summary>
///     The Extent of a <see cref="ObjectType" /> is the set of all objects that either
///     - are of the specified <see cref="ObjectType" />
///     - inherit from the specified <see cref="ObjectType" />
///     The extent can be filter based on predicates.
/// </summary>
/// <typeparam name="T">The .Net type of the extent.</typeparam>
public interface IExtent<out T> : IReadOnlyCollection<T> where T : class, IObject 
{
    /// <summary>
    ///     Gets the filter.
    /// </summary>
    /// <value>
    ///     The filter is a top level AND filter. If you require an OR or a NOT filter
    ///     then simply add it to this AND filter.
    /// </value>
    ICompositePredicate Filter { get; }

    /// <summary>
    ///     Gets the object type of this extent.
    /// </summary>
    /// <value>The type of the Extent.</value>
    Composite ObjectType { get; }

    /// <summary>
    ///     Adds sorting based on the specified relation type.
    /// </summary>
    /// <param name="roleType">The role type by which to sort.</param>
    /// <returns>The current extent.</returns>
    IExtent<IObject> AddSort(RoleType roleType);

    /// <summary>
    ///     Adds sorting based on the specified role type and direction.
    /// </summary>
    /// <param name="roleType">The role type by which to sort.</param>
    /// <param name="direction">The sort direction.</param>
    /// <returns>The current extent.</returns>
    IExtent<IObject> AddSort(RoleType roleType, SortDirection direction);
}

// <copyright file="ExtentT.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ExtentT type.</summary>

namespace Allors.Database;

public interface IFilter<out T> : IExtent<T> where T : class, IObject
{
    /// <summary>
    ///     Gets the predicate.
    /// </summary>
    /// <value>
    ///     The filter is a top level AND filter. If you require an OR or a NOT filter
    ///     then simply add it to this AND filter.
    /// </value>
    ICompositePredicate Predicate { get; }
}

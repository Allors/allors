// <copyright file="Extent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>


namespace Allors.Database;

using Allors.Database.Meta;

/// <summary>
///     The Extent of a <see cref="Meta.ObjectType" /> is the set of all objects that either
///     - are of the specified <see cref="Meta.ObjectType" />
///     - inherit from the specified <see cref="Meta.ObjectType" />
///     The extent can be filtered based on predicates.
/// </summary>
public interface Extent : Extent<IObject>
{
}

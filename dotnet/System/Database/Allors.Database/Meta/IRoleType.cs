// <copyright file="IRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;

/// <summary>
///     A <see cref="IRoleType" /> defines the role side of a relation.
///     This is also called the 'passive' side.
///     RoleTypes can have composite and unit <see cref="ObjectType" />s.
/// </summary>
public interface IRoleType : IPropertyType, IComparable
{
    IAssociationType AssociationType { get; }

    IRelationType RelationType { get; }

    string AssignedSingularName { get; }

    string AssignedPluralName { get; }

    string FullName { get; }

    int? Size { get; }

    int? Precision { get; }

    int? Scale { get; }

    bool IsRequired { get; set; }

    bool IsUnique { get; set; }

    string MediaType { get; set; }

    // TODO: move to extension method
    void Set(IStrategy strategy, object value);
}

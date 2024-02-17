// <copyright file="IRoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

/// <summary>
///     A <see cref="IRoleType" /> defines the role side of a relation.
///     This is also called the 'passive' side.
///     RoleTypes can have composite and unit <see cref="ObjectType" />s.
/// </summary>
public interface IRoleType : IRelationEndType, IComparable
{
    ICompositeRoleType CompositeRoleType { get; internal set; }

    IReadOnlyDictionary<IComposite, ICompositeRoleType> CompositeRoleTypeByComposite { get; }

    IAssociationType AssociationType { get; }

    IRelationType RelationType { get; internal set; }

    string AssignedSingularName { get; }

    string AssignedPluralName { get; }

    string FullName { get; }

    int? Size { get; }

    int? Precision { get; }

    int? Scale { get; }

    new string SingularName { get; internal set; }

    new string PluralName { get; }
    
    internal void InitializeCompositeRoleTypes(Dictionary<IComposite, HashSet<ICompositeRoleType>> compositeRoleTypesByComposite);

    internal void DeriveScaleAndSize();

    internal void DeriveIsRequired();

    internal void DeriveIsUnique();
}

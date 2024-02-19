﻿// <copyright file="IRelationType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta;

using System;

/// <summary>
///     A relation type defines the state and behavior for
///     a set of association types and role types.
/// </summary>
public interface IRelationType : IMetaIdentifiableObject, IComparable
{
    IAssociationType AssociationType { get; internal set; }

    IRoleType RoleType { get; internal set; }

    Multiplicity Multiplicity { get; }

    bool ExistExclusiveClasses { get; }

    bool IsDerived { get; }

    bool IsKey { get; set; }
    
    string Name { get; }

    internal void DeriveWorkspaceNames();
}

﻿// <copyright file="IRelationEndType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

/// <summary>
///     A <see cref="IRelationEndType" /> can be a <see cref="AssociationType" /> or a <see cref="RoleType" />.
/// </summary>
public interface IRelationEndType : IOperandType
{
    IObjectType ObjectType { get; }

    string Name { get; }

    string SingularName { get; }

    string SingularFullName { get; }

    string PluralName { get; }

    string PluralFullName { get; }

    bool IsOne { get; }

    bool IsMany { get; }

    internal void Validate(ValidationLog validationLog);
}

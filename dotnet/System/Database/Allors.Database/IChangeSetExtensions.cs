// <copyright file="IChangeSetExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSetExtensions type.</summary>

namespace Allors.Database;

using System.Collections.Generic;
using Allors.Collections;
using Allors.Database.Meta;

public static class IChangeSetExtensions
{
    private static readonly ISet<RoleType> EmptyRoleTypeSet = new EmptySet<RoleType>();

    private static readonly ISet<AssociationType> EmptyAssociationTypeSet = new EmptySet<AssociationType>();

    public static ISet<RoleType> GetRoleTypes(this IChangeSet @this, IObject association) =>
        @this.RoleTypesByAssociation.TryGetValue(association, out var roleTypes) ? roleTypes : EmptyRoleTypeSet;

    public static ISet<AssociationType> GetAssociationTypes(this IChangeSet @this, IObject role) =>
        @this.AssociationTypesByRole.TryGetValue(role, out var associationTypes) ? associationTypes : EmptyAssociationTypeSet;
}

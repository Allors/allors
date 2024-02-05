// <copyright file="Composite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IStaticComposite : IStaticObjectType, IComposite
{
    new IReadOnlyList<IInterface> Supertypes { get; internal set; }

    new IReadOnlyList<IRoleType> RoleTypes { get; internal set; }

    new IReadOnlyList<IAssociationType> AssociationTypes { get; internal set; }

    new IReadOnlyList<IMethodType> MethodTypes { get; internal set; }

    new IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType { get; internal set; }

    new IReadOnlyDictionary<IMethodType, ICompositeMethodType> CompositeMethodTypeByMethodType { get; internal set; }

    internal IRoleType DerivedKeyRoleType { get; set; }
}

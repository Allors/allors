// <copyright file="IComposite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IComposite : IObjectType
{
    IReadOnlyList<IInterface> DirectSupertypes { get; }

    IReadOnlyList<IInterface> Supertypes { get; }

    IReadOnlyList<IComposite> DirectSubtypes { get; }

    IReadOnlyList<IComposite> Subtypes { get; }

    IReadOnlyList<IComposite> Composites { get; }

    IReadOnlyList<IClass> Classes { get; }

    IClass ExclusiveClass { get; }

    IReadOnlyList<IAssociationType> AssociationTypes { get; }

    IReadOnlyList<IRoleType> RoleTypes { get; }

    IReadOnlyDictionary<IRoleType, ICompositeRoleType> CompositeRoleTypeByRoleType { get; }

    IRoleType KeyRoleType { get; }

    IReadOnlyList<IMethodType> MethodTypes { get; }

    IReadOnlyDictionary<IMethodType, ICompositeMethodType> CompositeMethodTypeByMethodType { get; }

    bool IsAssignableFrom(IComposite objectType);
}

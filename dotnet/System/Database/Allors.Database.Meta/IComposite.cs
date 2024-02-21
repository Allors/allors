// <copyright file="IComposite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IComposite : IObjectType
{
    IReadOnlyList<Interface> DirectSupertypes { get; set; }

    IReadOnlyList<Interface> Supertypes { get; internal set; }

    IReadOnlyList<IComposite> DirectSubtypes { get; }

    IReadOnlyList<IComposite> Subtypes { get; }

    IReadOnlyList<IComposite> Composites { get; }

    IReadOnlyList<Class> Classes { get; }

    Class ExclusiveClass { get; }

    IReadOnlyList<AssociationType> AssociationTypes { get; internal set; }

    IReadOnlyList<RoleType> RoleTypes { get; internal set; }

    IReadOnlyDictionary<RoleType, CompositeRoleType> CompositeRoleTypeByRoleType { get; internal set; }

    RoleType KeyRoleType { get; }

    IReadOnlyList<MethodType> MethodTypes { get; internal set; }

    IReadOnlyDictionary<MethodType, CompositeMethodType> CompositeMethodTypeByMethodType { get; internal set; }

    bool IsAssignableFrom(IComposite objectType);

    internal RoleType DerivedKeyRoleType { get; set; }
}

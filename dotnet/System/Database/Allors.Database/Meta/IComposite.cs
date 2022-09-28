// <copyright file="IComposite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IComposite : IObjectType
{
    IEnumerable<IInterface> DirectSupertypes { get; }

    IEnumerable<IInterface> Supertypes { get; }

    IEnumerable<IComposite> Subtypes { get; }

    IEnumerable<IClass> Classes { get; }

    IClass ExclusiveClass { get; }

    IEnumerable<IAssociationType> AssociationTypes { get; }

    IEnumerable<IRoleType> RoleTypes { get; }

    IEnumerable<IMethodType> MethodTypes { get; }

    bool ExistClass { get; }

    bool ExistExclusiveClass { get; }

    bool ExistSupertype(IInterface @interface);

    bool ExistAssociationType(IAssociationType association);

    bool ExistRoleType(IRoleType roleType);

    bool IsAssignableFrom(IComposite objectType);
}

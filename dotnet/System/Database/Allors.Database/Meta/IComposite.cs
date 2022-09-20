// <copyright file="IComposite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public interface IComposite : IObjectType
{
    IEnumerable<IInterface> Supertypes { get; }

    IEnumerable<IComposite> Subtypes { get; }

    bool ExistClass { get; }

    IEnumerable<IClass> Classes { get; }

    bool ExistExclusiveClass { get; }

    IClass ExclusiveClass { get; }

    IEnumerable<IAssociationType> AssociationTypes { get; }

    IEnumerable<IAssociationType> InheritedAssociationTypes { get; }

    IEnumerable<IAssociationType> ExclusiveAssociationTypes { get; }

    IEnumerable<IRoleType> RoleTypes { get; }

    IEnumerable<IRoleType> InheritedRoleTypes { get; }

    IEnumerable<IRoleType> ExclusiveRoleTypes { get; }

    IEnumerable<IMethodType> MethodTypes { get; }

    IEnumerable<IMethodType> InheritedMethodTypes { get; }

    IEnumerable<IMethodType> ExclusiveMethodTypes { get; }

    bool ExistSupertype(IInterface @interface);

    bool ExistAssociationType(IAssociationType association);

    bool ExistRoleType(IRoleType roleType);

    bool IsAssignableFrom(IComposite objectType);
}

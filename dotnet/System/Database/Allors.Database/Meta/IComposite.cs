// <copyright file="IComposite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IComposite : IObjectType
{
    IReadOnlySet<IInterface> DirectSupertypes { get; }

    IReadOnlySet<IInterface> Supertypes { get; }

    IReadOnlySet<IComposite> Subtypes { get; }

    IReadOnlySet<IClass> Classes { get; }

    IClass ExclusiveClass { get; }

    IReadOnlySet<IAssociationType> AssociationTypes { get; }

    IReadOnlySet<IRoleType> RoleTypes { get; }

    IReadOnlySet<IMethodType> MethodTypes { get; }

    bool IsAssignableFrom(IComposite objectType);
}

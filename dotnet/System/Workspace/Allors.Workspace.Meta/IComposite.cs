// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;

    public interface IComposite : IObjectType
    {
        IReadOnlyList<IInterface> DirectSupertypes { get; }

        IReadOnlyList<IInterface> Supertypes { get; }

        IReadOnlyList<IClass> Classes { get; }

        IReadOnlyList<IAssociationType> AssociationTypes { get; }

        IReadOnlyList<IRoleType> RoleTypes { get; }

        IReadOnlyList<IMethodType> MethodTypes { get; }

        bool IsAssignableFrom(IComposite objectType);

        void Bind(Dictionary<string, Type> typeByName);
    }
}

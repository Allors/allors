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
        ISet<Interface> DirectSupertypes { get; } 

        ISet<Interface> Supertypes { get; set; } // TODO: move set;

        ISet<Class> Classes { get; }

        ISet<AssociationType> AssociationTypes { get; }

        ISet<RoleType> RoleTypes { get; }

        ISet<RoleType> DatabaseOriginRoleTypes { get; }

        ISet<MethodType> MethodTypes { get; }

        RoleType[] ExclusiveRoleTypes { get; set; }

        AssociationType[] ExclusiveAssociationTypes { get; set; }

        MethodType[] ExclusiveMethodTypes { get; set; }

        bool IsAssignableFrom(IComposite objectType);

        void Bind(Dictionary<string, Type> typeByName);
    }
}

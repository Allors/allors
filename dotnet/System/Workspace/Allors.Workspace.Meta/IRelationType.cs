// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;

    public interface IRelationType : IMetaIdentifiableObject
    {
        IAssociationType AssociationType { get; }

        IRoleType RoleType { get; }

        Multiplicity Multiplicity { get; }

        bool IsDerived { get; }
    }
}

// <copyright file="IMetaCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Services;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public interface IMetaCache
{
    Type GetBuilderType(Class @class);

    IReadOnlySet<Interface> GetSupertypesByComposite(IComposite composite);

    IReadOnlySet<AssociationType> GetAssociationTypesByComposite(IComposite composite);

    IReadOnlySet<RoleType> GetRoleTypesByComposite(IComposite composite);

    IReadOnlySet<RoleType> GetRequiredRoleTypesByComposite(IComposite composite);

    IReadOnlySet<CompositeRoleType> GetRequiredCompositeRoleTypesByClass(Class @class);

    IReadOnlySet<Class> GetWorkspaceClasses(string workspaceName);

    IDictionary<Class, IReadOnlySet<RoleType>> GetWorkspaceRoleTypesByClass(string workspaceName);
}

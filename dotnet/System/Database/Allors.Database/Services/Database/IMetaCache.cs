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
    Type GetBuilderType(IClass @class);

    IReadOnlySet<Interface> GetSupertypesByComposite(IComposite composite);

    IReadOnlySet<IAssociationType> GetAssociationTypesByComposite(IComposite composite);

    IReadOnlySet<IRoleType> GetRoleTypesByComposite(IComposite composite);

    IReadOnlySet<IRoleType> GetRequiredRoleTypesByComposite(IComposite composite);

    IReadOnlySet<ICompositeRoleType> GetRequiredCompositeRoleTypesByClass(IClass @class);

    IReadOnlySet<IClass> GetWorkspaceClasses(string workspaceName);

    IDictionary<IClass, IReadOnlySet<IRoleType>> GetWorkspaceRoleTypesByClass(string workspaceName);
}

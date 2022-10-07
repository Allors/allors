// <copyright file="IMetaCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Services;

using System;
using System.Collections.Generic;
using Meta;

public interface IMetaCache
{
    Type GetBuilderType(IClass @class);

    IReadOnlySet<IInterface> GetSupertypesByComposite(IComposite composite);

    IReadOnlySet<IAssociationType> GetAssociationTypesByComposite(IComposite composite);

    IReadOnlySet<IRoleType> GetRoleTypesByComposite(IComposite composite);

    IReadOnlySet<IRoleType> GetRequiredRoleTypesByComposite(IComposite composite);

    IReadOnlySet<ICompositeRoleType> GetRequiredCompositeRoleTypesByClass(IClass @class);

    IReadOnlySet<IClass> GetWorkspaceClasses(string workspaceName);

    IDictionary<IClass, IReadOnlySet<IRoleType>> GetWorkspaceRoleTypesByClass(string workspaceName);
}

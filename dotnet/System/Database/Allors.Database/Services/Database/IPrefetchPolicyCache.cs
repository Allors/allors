// <copyright file="IPreparedSelects.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain;

using System.Collections.Generic;
using Allors.Database.Data;
using Allors.Database.Meta;

public interface IPrefetchPolicyCache
{
    PrefetchPolicy PermissionsWithClass { get; }

    PrefetchPolicy Security { get; }

    PrefetchPolicy ForDependency(IComposite composite, ISet<IRelationEndType> relationEndTypes);

    IDictionary<IClass, PrefetchPolicy> WorkspacePrefetchPolicyByClass(string workspaceName);

    PrefetchPolicy ForNodes(Node[] nodes);
}

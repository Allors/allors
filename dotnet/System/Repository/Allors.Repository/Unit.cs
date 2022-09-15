// <copyright file="Unit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System.Collections.Generic;

    public class Unit : ObjectType
    {
        public Unit(ISet<RepositoryObject> objects, string name, Domain domain)
            : base(objects, name, domain)
        {
        }
    }
}

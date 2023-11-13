// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    using Allors.Database.Data;

    public partial class PersistentPreparedExtents
    {
        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().PersistentPreparedExtentByUniqueId().Merger().Action();

            merge(PersistentPreparedExtent.ByNameId, v =>
            {
                v.Description = "Organisation by name";
                v.Extent = new Extent(this.M.Organisation) { Predicate = new Equals(this.M.Organisation.Name) { Parameter = "name" } };
            });
        }
    }
}

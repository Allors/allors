// <copyright file="Organizations.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Data;

    public partial class PersistentPreparedExtents
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().PersistentPreparedExtentByUniqueId().Merger().Action();

            merge(PersistentPreparedExtent.ByNameId, v =>
            {
                v.Description = "Organization by name";
                v.Extent = new Extent(this.M.Organization) { Predicate = new Equals(this.M.Organization.Name) { Parameter = "name" } };
            });
        }
    }
}

// <copyright file="Organizations.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Data;

    public partial class PersistentPreparedSelects
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().PersistentPreparedSelectByUniqueId().Merger().Action();

            merge(PersistentPreparedSelect.SelectPeopleId, v =>
            {
                v.Description = "Select People";
                v.Select = new Select
                {
                    Include =
                    [
                        new Node(this.M.Organization.Owner),
                        new Node(this.M.Organization.Employees),
                    ],
                };
            });
        }
    }
}

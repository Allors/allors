// <copyright file="Genders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class Genders
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().GenderByUniqueId().Merger().Action();

            merge(Gender.MaleId, v =>
            {
                v.Name = "Male";
            });

            merge(Gender.FemaleId, v =>
            {
                v.Name = "Female";
            });

            merge(Gender.OtherId, v =>
            {
                v.Name = "Other";
            });

            merge(Gender.PreferNotToSayId, v =>
            {
                v.Name = "Prefer not to say";
            });
        }
    }
}

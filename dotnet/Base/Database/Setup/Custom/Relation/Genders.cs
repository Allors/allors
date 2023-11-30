// <copyright file="Genders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Genders
    {
        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var merge = this.Transaction.Caches().GenderByKey().Merger().Action();

            merge(Gender.MaleId, v => { });

            merge(Gender.FemaleId, v => { });

            merge(Gender.OtherId, v => { });

            merge(Gender.PreferNotToSayId, v => { });
        }
    }
}

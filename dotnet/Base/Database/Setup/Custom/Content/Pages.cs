// <copyright file="Two.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Pages
    {
        protected override void CustomPrepare(Setup setup)
        {
            base.CustomPrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.Media);
        }

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var medias = this.Transaction.Scoped<MediaByUniqueId>();

            var merge = this.Transaction.Caches().PageByUniqueId().Merger().Action();

            merge(Page.IndexId, v =>
            {
                v.Name = "About";
                v.Content = medias.About;
            });
        }
    }
}

// <copyright file="Templates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Templates
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, this.M.TemplateType);
        }
    }
}

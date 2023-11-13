// <copyright file="TemplateTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class TemplateTypes
    {
        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().TemplateTypeByUniqueId().Merger().Action();

            merge(TemplateType.OpenDocumentTypeId, v => v.Name = "Odt Template");
        }
    }
}

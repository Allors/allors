// <copyright file="TemplateTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;


    public partial class TemplateTypes
    {
        private ICache<Guid, TemplateType> cache;

        public ICache<Guid, TemplateType> Cache => this.cache ??= this.Transaction.Caches().TemplateTypeByUniqueId();

        public TemplateType OpenDocumentType => this.Cache[TemplateType.OpenDocumentTypeId];

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger(v => v.IsActive = true).Action();

            merge(TemplateType.OpenDocumentTypeId, v => v.Name = "Odt Template");
        }
    }
}

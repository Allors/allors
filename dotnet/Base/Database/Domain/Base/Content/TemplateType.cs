// <copyright file="TemplateType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class TemplateType
    {
        public static readonly Guid OpenDocumentTypeId = new Guid("64B48FA3-EDF2-45A3-ADFB-4A55E14E0B34");

        public bool IsOpenDocumentTemplate => this.UniqueId.Equals(OpenDocumentTypeId);
    }
}

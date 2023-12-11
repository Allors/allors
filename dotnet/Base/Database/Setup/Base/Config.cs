// <copyright file="Config.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;
    using Meta;

    public partial record Config
    {
        public IDictionary<IClass, IDictionary<IRoleType, IDictionary<CultureInfo, ResourceSet>>> ResourceSetByCultureInfoByRoleTypeByClass { get; set; }
    }
}

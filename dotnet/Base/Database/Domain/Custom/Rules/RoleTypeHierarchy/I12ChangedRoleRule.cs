// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Derivations;
    using Allors.Database.Domain.Derivations.Rules;

    public class I12ChangedRoleRule : Rule<I12>
    {
        public I12ChangedRoleRule(IMetaIndex m) : base(m, new Guid("48656EC9-5331-4AC6-B899-738D1983FD5F")) =>
            this.Patterns =
            [
                new RolePattern<S12, I12>(m.S12.ChangedRolePingI12, v=> v as I12) ,
            ];


        public override void Derive(ICycle cycle, IEnumerable<I12> matches)
        {
            foreach (var s12 in matches)
            {
                s12.ChangedRolePongI12 = s12.ChangedRolePingI12;
            }
        }
    }
}

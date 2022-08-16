// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Derivations.Rules;
    using Meta;

    public class RevocationSecurityStampRule : Rule
    {
        public RevocationSecurityStampRule(MetaPopulation m) : base(m, new Guid("A46A6B64-B887-4388-85BB-ACE6DB452F00")) =>
            this.Patterns = new Pattern[]
            {
                m.Revocation.RolePattern(v=>v.DeniedPermissions),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            var validation = cycle.Validation;

            foreach (var revocation in matches.Cast<Revocation>())
            {
                revocation.DeriveRevocationSecurityStampRule(validation);
            }
        }
    }

    public static class RevocationSecurityStampRuleExtensions
    {
        public static void DeriveRevocationSecurityStampRule(this Revocation @this, IValidation validation) => @this.SecurityStamp = Guid.NewGuid();
    }
}

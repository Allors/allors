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

    public class DelegatedAccessSecurityStampRule : Rule
    {
        public DelegatedAccessSecurityStampRule(MetaPopulation m) : base(m, new Guid("6999F66D-B567-490A-A009-7756CF8E580C")) =>
            this.Patterns = new Pattern[]
            {
                m.DelegatedAccess.RolePattern(v=>v.DelegatedSecurityTokens),
                m.DelegatedAccess.RolePattern(v=>v.DelegatedRevocations),
                m.SecurityToken.RolePattern(v=>v.SecurityStamp, v => v.DelegatedAccessesWhereDelegatedSecurityToken),
                m.Revocation.RolePattern(v=>v.SecurityStamp, v => v.DelegatedAccessesWhereDelegatedRevocation),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            var validation = cycle.Validation;

            foreach (var delegatedAccess in matches.Cast<DelegatedAccess>())
            {
                delegatedAccess.DeriveDelegatedAccessSecurityStampRule(validation);
            }
        }
    }

    public static class DelegatedAccessSecurityStampRuleExtensions
    {
        public static void DeriveDelegatedAccessSecurityStampRule(this DelegatedAccess @this, IValidation validation) => @this.SecurityStamp = Guid.NewGuid();
    }
}

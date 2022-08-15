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

    public class SecurityTokenGroupSecurityStampRule : Rule
    {
        public SecurityTokenGroupSecurityStampRule(MetaPopulation m) : base(m, new Guid("6999F66D-B567-490A-A009-7756CF8E580C")) =>
            this.Patterns = new Pattern[]
            {
                m.SecurityTokenGroup.RolePattern(v=>v.SecurityTokens),
                m.SecurityToken.RolePattern(v=>v.SecurityStamp, v => v.SecurityTokenGroupsWhereMember),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            var validation = cycle.Validation;

            foreach (var securityTokenGroup in matches.Cast<SecurityTokenGroup>())
            {
                securityTokenGroup.DeriveSecurityTokenGroupSecurityStampRule(validation);
            }
        }
    }

    public static class SecurityTokenGroupSecurityStampRuleExtensions
    {
        public static void DeriveSecurityTokenGroupSecurityStampRule(this SecurityTokenGroup @this, IValidation validation) => @this.SecurityStamp = Guid.NewGuid();
    }
}

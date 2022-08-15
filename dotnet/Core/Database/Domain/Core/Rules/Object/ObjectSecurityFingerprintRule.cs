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

    public class ObjectSecurityFingerprintRule : Rule
    {
        public ObjectSecurityFingerprintRule(MetaPopulation m) : base(m, new Guid("24C833AD-8588-49EA-9D93-5A2C56EA4E9B")) =>
            this.Patterns = new Pattern[]
            {
                m.Object.RolePattern(v=>v.SecurityTokens),
                m.Object.RolePattern(v=>v.SharedSecurity),
                m.SecurityToken.RolePattern(v=>v.SecurityStamp, v=>v.ObjectsWhereSecurityToken),
                m.SecurityTokenGroup.RolePattern(v=>v.SecurityStamp, v => v.ObjectsWhereSharedSecurity),
            };

        public override void Derive(ICycle cycle, IEnumerable<IObject> matches)
        {
            var validation = cycle.Validation;

            foreach (var securityToken in matches.Cast<SecurityToken>())
            {
                securityToken.DeriveObjectSecurityFingerprintRule(validation);
            }
        }
    }

    public static class ObjectSecurityFingerprintRuleExtensions
    {
        public static void DeriveObjectSecurityFingerprintRule(this Object @this, IValidation validation) => @this.SecurityFingerPrint = Guid.NewGuid();
    }
}

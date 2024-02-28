// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System.Collections.Generic;
    using Database.Derivations;
    using Meta;

    public class RolePattern: IRolePattern
    {
        public RolePattern(RoleType roleType)
        {
            this.RoleType = roleType;
        }

        public RoleType RoleType { get; }

        public IEnumerable<IObject> Eval(IObject @object)
        {
            // TODO: Type check
            if (@object != null)
            {
                yield return @object;
            }
        }
    }
}

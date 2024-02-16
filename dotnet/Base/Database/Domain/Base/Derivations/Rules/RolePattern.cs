﻿// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public class RolePattern : Pattern, IRolePattern
    {
        public RolePattern(IRoleType roleType) => this.RoleType = roleType;

        public RolePattern(IComposite objectType, IRoleType roleType)
        {
            this.RoleType = roleType;
            this.OfType = !this.RoleType.AssociationType.ObjectType.Equals(objectType) ? objectType : null;
        }

        public IRoleType RoleType { get; }
    }
}

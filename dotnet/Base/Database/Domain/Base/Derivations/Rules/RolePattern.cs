// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System.Collections.Generic;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;
    using Database.Data;

    public class RolePattern : IRolePattern
    {
        public RolePattern(IRoleType roleType, IComposite ofType = null) 
        {
            this.RoleType = roleType;
            this.OfType = !this.RoleType.AssociationType.ObjectType.Equals(ofType) ? ofType : null;
        }

        public IEnumerable<Node> Tree { get; set; }

        public IComposite OfType { get; set; }
        
        public IRoleType RoleType { get; }
    }
}

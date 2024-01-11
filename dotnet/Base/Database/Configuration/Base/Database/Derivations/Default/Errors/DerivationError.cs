// <copyright file="DerivationError.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using System.Collections.Generic;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public abstract class DerivationError : IDerivationError
    {
        protected DerivationError(IValidation validation, IDerivationRelation[] relations, string errorCode)
        {
            this.Validation = validation;
            this.Relations = relations;
            this.ErrorCode = errorCode;
        }

        public IValidation Validation { get; }

        public IDerivationRelation[] Relations { get; }

        public string ErrorCode { get; }

        public IRoleType[] RoleTypes
        {
            get
            {
                var roleTypes = new List<IRoleType>();
                foreach (var relation in this.Relations)
                {
                    var roleType = relation.RelationType.RoleType;
                    if (!roleTypes.Contains(roleType))
                    {
                        roleTypes.Add(roleType);
                    }
                }

                return roleTypes.ToArray();
            }
        }
    }
}

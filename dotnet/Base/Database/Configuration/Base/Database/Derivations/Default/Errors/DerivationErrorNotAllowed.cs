// <copyright file="DerivationErrorNotAllowed.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Database.Configuration.Derivations.Default
{
    using Allors.Database.Derivations;
    using Allors.Database.Meta;
    using Domain;

    public class DerivationErrorNotAllowed : DerivationError, IDerivationErrorNotAllowed
    {
        public DerivationErrorNotAllowed(IValidation validation, IDerivationRelation relation)
            : base(validation, [relation], ErrorCodes.DerivationErrorNotAllowed)
        {
        }

        public DerivationErrorNotAllowed(IValidation validation, IObject association, RoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }
    }
}

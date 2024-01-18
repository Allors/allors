﻿// <copyright file="DerivationErrorConflict.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using Allors.Database.Derivations;
    using Allors.Database.Meta;
    using Domain;

    public class DerivationErrorConflict : DerivationError, IDerivationErrorConflict
    {
        public DerivationErrorConflict(IValidation validation, IDerivationRelation relation)
            : base(validation, new[] { relation }, ErrorCodes.DerivationErrorConflict)
        {
        }

        public DerivationErrorConflict(IValidation validation, IObject association, IRoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }
    }
}
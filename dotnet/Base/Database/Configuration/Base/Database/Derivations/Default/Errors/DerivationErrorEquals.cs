﻿// <copyright file="DerivationErrorEquals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using Allors.Database.Derivations;
    using Domain;

    public class DerivationErrorEquals : DerivationError, IDerivationErrorEquals
    {
        public DerivationErrorEquals(IValidation validation, IDerivationRelation[] derivationRelations)
            : base(validation, derivationRelations, ErrorCodes.DerivationErrorEquals)
        {
        }
    }
}

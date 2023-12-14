﻿// <copyright file="DerivationErrorAtLeastOne.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using Allors.Database.Derivations;
    using Domain;

    public class DerivationErrorAtLeastOne : DerivationError, IDerivationErrorAtLeastOne
    {
        public DerivationErrorAtLeastOne(IValidation validation, IDerivationRelation[] derivationRelations)
            : base(validation, derivationRelations, ErrorCodes.DerivationErrorAtLeastOne)
        {
        }
    }
}

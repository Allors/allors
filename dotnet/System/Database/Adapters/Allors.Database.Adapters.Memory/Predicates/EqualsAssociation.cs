﻿// <copyright file="AssociationEquals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class EqualsAssociation : Equals
{
    private readonly AssociationType associationType;
    private readonly IObject equals;

    internal EqualsAssociation(IInternalExtent extent, AssociationType associationType, IObject equals)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationEquals(associationType, equals);

        this.associationType = associationType;
        this.equals = equals;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var association = strategy.GetCompositeAssociation(this.associationType);
        return association?.Equals(this.equals) == true
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

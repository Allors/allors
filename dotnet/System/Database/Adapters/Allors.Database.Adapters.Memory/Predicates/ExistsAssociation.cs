// <copyright file="AssociationExists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class ExistsAssociation : Exists
{
    private readonly AssociationType associationType;

    internal ExistsAssociation(IInternalExtent extent, AssociationType associationType)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.ValidateAssociationExists(associationType);

        this.associationType = associationType;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        if (this.associationType.IsMany)
        {
            return strategy.ExistCompositesAssociation(this.associationType)
                ? ThreeValuedLogic.True
                : ThreeValuedLogic.False;
        }

        return strategy.ExistCompositeAssociation(this.associationType)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

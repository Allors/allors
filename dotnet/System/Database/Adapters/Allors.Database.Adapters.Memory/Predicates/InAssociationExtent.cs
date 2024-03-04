// <copyright file="InAssociationExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class InAssociationExtent : In
{
    private readonly AssociationType associationType;
    private readonly Allors.Database.Extent containingExtent;

    internal InAssociationExtent(ExtentFiltered extent, AssociationType associationType, Allors.Database.Extent containingExtent)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationIn(associationType, containingExtent);

        this.associationType = associationType;
        this.containingExtent = containingExtent;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        if (this.associationType.IsMany)
        {
            foreach (var assoc in strategy.GetCompositesAssociation<IObject>(this.associationType))
            {
                if (this.containingExtent.Contains(assoc))
                {
                    return ThreeValuedLogic.True;
                }
            }

            return ThreeValuedLogic.False;
        }

        var association = strategy.GetCompositeAssociation(this.associationType);
        if (association != null)
        {
            return this.containingExtent.Contains(association)
                ? ThreeValuedLogic.True
                : ThreeValuedLogic.False;
        }

        return ThreeValuedLogic.False;
    }
}

// <copyright file="InAssociationExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class IntersectsAssociationExtent : In
{
    private readonly AssociationType associationType;
    private readonly Allors.Database.IExtent<IObject> containingExtent;

    internal IntersectsAssociationExtent(IInternalExtent extent, AssociationType associationType, Allors.Database.IExtent<IObject> containingExtent)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationIn(associationType, containingExtent);

        this.associationType = associationType;
        this.containingExtent = containingExtent;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
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
}

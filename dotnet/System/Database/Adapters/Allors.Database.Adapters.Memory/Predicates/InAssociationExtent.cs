// <copyright file="InAssociationExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class InAssociationExtent : In
{
    private readonly AssociationType associationType;
    private readonly Allors.Database.IExtent<IObject> containingExtent;

    internal InAssociationExtent(IInternalExtent extent, AssociationType associationType, Allors.Database.IExtent<IObject> containingExtent)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationIn(associationType, containingExtent);

        this.associationType = associationType;
        this.containingExtent = containingExtent;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
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

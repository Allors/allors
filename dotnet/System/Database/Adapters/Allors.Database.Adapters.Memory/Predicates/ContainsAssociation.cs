// <copyright file="AssociationContains.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Linq;
using Allors.Database.Meta;

internal sealed class ContainsAssociation : Contains
{
    private readonly AssociationType associationType;
    private readonly IObject containedObject;

    internal ContainsAssociation(IInternalExtent extent, AssociationType associationType, IObject containedObject)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationContains(associationType, containedObject);

        this.associationType = associationType;
        this.containedObject = containedObject;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy) =>
        strategy.GetCompositesAssociation<IObject>(this.associationType).Contains(this.containedObject)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
}

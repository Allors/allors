// <copyright file="AssociationContainedInEnumerable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class ContainedInAssociationEnumerable : ContainedIn
{
    private readonly AssociationType associationType;
    private readonly IEnumerable<IObject> containingEnumerable;

    internal ContainedInAssociationEnumerable(ExtentFiltered extent, AssociationType associationType,
        IEnumerable<IObject> containingEnumerable)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.AssertAssociationContainedIn(associationType, containingEnumerable);

        this.associationType = associationType;
        this.containingEnumerable = containingEnumerable;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var containing = new HashSet<IObject>(this.containingEnumerable);

        if (this.associationType.IsMany)
        {
            foreach (var assoc in strategy.GetCompositesAssociation<IObject>(this.associationType))
            {
                if (containing.Contains(assoc))
                {
                    return ThreeValuedLogic.True;
                }
            }

            return ThreeValuedLogic.False;
        }

        var association = strategy.GetCompositeAssociation(this.associationType);
        if (association != null)
        {
            return containing.Contains(association)
                ? ThreeValuedLogic.True
                : ThreeValuedLogic.False;
        }

        return ThreeValuedLogic.False;
    }
}

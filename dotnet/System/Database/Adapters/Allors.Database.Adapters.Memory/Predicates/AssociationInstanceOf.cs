﻿// <copyright file="AssociationInstanceOf.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using Allors.Database.Meta;

internal sealed class AssociationInstanceOf : Predicate
{
    private readonly AssociationType associationType;
    private readonly IObjectType objectType;

    internal AssociationInstanceOf(ExtentFiltered extent, AssociationType associationType, IObjectType instanceObjectType)
    {
        extent.CheckForAssociationType(associationType);
        PredicateAssertions.ValidateAssociationInstanceof(associationType, instanceObjectType);

        this.associationType = associationType;
        this.objectType = instanceObjectType;
    }

    internal override ThreeValuedLogic Evaluate(Strategy strategy)
    {
        var association = strategy.GetCompositeAssociation(this.associationType);

        if (association == null)
        {
            return ThreeValuedLogic.False;
        }

        // TODO: Optimize
        var associationObjectType = association.Strategy.Class;
        if (associationObjectType.Equals(this.objectType))
        {
            return ThreeValuedLogic.True;
        }

        var metaCache = strategy.Transaction.Database.MetaCache;

        return this.objectType is Interface @interface && metaCache.GetSupertypesByComposite(associationObjectType).Contains(@interface)
            ? ThreeValuedLogic.True
            : ThreeValuedLogic.False;
    }
}

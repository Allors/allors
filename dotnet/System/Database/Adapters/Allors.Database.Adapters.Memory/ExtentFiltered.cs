﻿// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class ExtentFiltered : Extent, IInternalExtent
{
    private And filter;

    internal ExtentFiltered(Transaction transaction, Composite objectType)
        : base(transaction) =>
        this.ObjectType = objectType;

    public override ICompositePredicate Filter
    {
        get
        {
            this.filter ??= new And(this);
            return this.filter;
        }
    }

    public override Composite ObjectType { get; }

    public void CheckForAssociationType(AssociationType association)
    {
        if (!this.Transaction.Database.MetaCache.GetAssociationTypesByComposite(this.ObjectType).Contains(association))
        {
            throw new ArgumentException("Extent does not have association " + association);
        }
    }

    public void CheckForRoleType(RoleType roleType)
    {
        if (!this.Transaction.Database.MetaCache.GetRoleTypesByComposite(this.ObjectType).Contains(roleType))
        {
            throw new ArgumentException("Extent does not have role " + roleType.SingularName);
        }
    }

    protected override void Evaluate()
    {
        if (this.Strategies == null)
        {
            this.Strategies = new List<Strategy>();

            foreach (var strategy in this.Transaction.GetStrategiesForExtentIncludingDeleted(this.ObjectType))
            {
                if (!strategy.IsDeleted)
                {
                    if (this.filter?.Include != true || this.filter.Evaluate(strategy) == ThreeValuedLogic.True)
                    {
                        this.Strategies.Add(strategy);
                    }
                }
            }

            if (this.Sorter != null)
            {
                this.Strategies.Sort(this.Sorter);
            }
        }
    }
}

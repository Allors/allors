// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

internal sealed class ExtentFiltered<T> : Extent<T> where T : class, IObject
{
    private And filter;

    public ExtentFiltered(Transaction transaction, Composite objectType)
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

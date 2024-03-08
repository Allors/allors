// <copyright file="Filter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Extent : IExtent, IPredicateContainer
{
    public Extent(Composite objectType) => this.ObjectType = objectType;

    public IPredicate Predicate { get; set; }

    public Composite ObjectType { get; set; }

    public Sort[] Sorting { get; set; }

    bool IExtent.HasMissingArguments(IArguments arguments) => this.Predicate != null && this.Predicate.HasMissingArguments(arguments);

    public Database.IExtent<IObject> Build(ITransaction transaction, IArguments arguments = null)
    {
        var extent = transaction.Filter(this.ObjectType);

        if (this.Predicate != null && !this.Predicate.ShouldTreeShake(arguments))
        {
            this.Predicate?.Build(transaction, arguments, extent);
        }

        if (this.Sorting != null)
        {
            foreach (var sort in this.Sorting)
            {
                sort.Build(extent);
            }
        }

        return extent;
    }

    public void Accept(IVisitor visitor) => visitor.VisitExtent(this);

    void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;
}

// <copyright file="Exists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Exists : IPropertyPredicate
{
    public Exists(IRelationEndType relationEndType = null) => this.RelationEndType = relationEndType;

    public string Parameter { get; set; }

    public IRelationEndType RelationEndType { get; set; }

    bool IPredicate.ShouldTreeShake(IArguments arguments) => ((IPredicate)this).HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && arguments?.HasArgument(this.Parameter) != true;

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        var relationEndType = this.Parameter != null
            ? (IRelationEndType)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter))
            : this.RelationEndType;

        if (relationEndType != null)
        {
            if (relationEndType is IRoleType roleType)
            {
                compositePredicate.AddExists(roleType);
            }
            else
            {
                var associationType = (IAssociationType)relationEndType;
                compositePredicate.AddExists(associationType);
            }
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitExists(this);
}

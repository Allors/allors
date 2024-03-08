// <copyright file="Exists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Exists(RelationEndType relationEndType = null) : IPropertyPredicate
{
    public string Parameter { get; set; }

    public RelationEndType RelationEndType { get; set; } = relationEndType;

    bool IPredicate.ShouldTreeShake(IArguments arguments) => ((IPredicate)this).HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && arguments?.HasArgument(this.Parameter) != true;

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        var relationEndType = this.Parameter != null
            ? (RelationEndType)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter))
            : this.RelationEndType;

        if (relationEndType != null)
        {
            if (relationEndType is RoleType roleType)
            {
                compositePredicate.AddExists(roleType);
            }
            else
            {
                var associationType = (AssociationType)relationEndType;
                compositePredicate.AddExists(associationType);
            }
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitExists(this);
}

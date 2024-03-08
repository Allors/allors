// <copyright file="Instanceof.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Instanceof(RelationEndType relationEndType = null) : IPropertyPredicate
{
    public string Parameter { get; set; }

    public Composite ObjectType { get; set; }

    public RelationEndType RelationEndType { get; set; } = relationEndType;

    bool IPredicate.ShouldTreeShake(IArguments arguments) => ((IPredicate)this).HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && arguments?.HasArgument(this.Parameter) != true;

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        var composite = this.Parameter != null
            ? (Composite)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter))
            : this.ObjectType;

        if (this.RelationEndType != null)
        {
            if (this.RelationEndType is RoleType roleType)
            {
                compositePredicate.AddInstanceOf(roleType, composite);
            }
            else
            {
                var associationType = (AssociationType)this.RelationEndType;
                compositePredicate.AddInstanceOf(associationType, composite);
            }
        }
        else
        {
            compositePredicate.AddInstanceOf(composite);
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitInstanceOf(this);
}

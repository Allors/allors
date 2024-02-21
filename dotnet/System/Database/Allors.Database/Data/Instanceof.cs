// <copyright file="Instanceof.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class Instanceof : IPropertyPredicate
{
    public Instanceof(IRelationEndType relationEndType = null) => this.RelationEndType = relationEndType;

    public string Parameter { get; set; }

    public IComposite ObjectType { get; set; }

    public IRelationEndType RelationEndType { get; set; }

    bool IPredicate.ShouldTreeShake(IArguments arguments) => ((IPredicate)this).HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && arguments?.HasArgument(this.Parameter) != true;

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        var composite = this.Parameter != null
            ? (IComposite)transaction.GetMetaObject(arguments.ResolveMetaObject(this.Parameter))
            : this.ObjectType;

        if (this.RelationEndType != null)
        {
            if (this.RelationEndType is RoleType roleType)
            {
                compositePredicate.AddInstanceof(roleType, composite);
            }
            else
            {
                var associationType = (AssociationType)this.RelationEndType;
                compositePredicate.AddInstanceof(associationType, composite);
            }
        }
        else
        {
            compositePredicate.AddInstanceof(composite);
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitInstanceOf(this);
}

// <copyright file="In.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using System.Collections.Generic;
using Allors.Database.Meta;

public class In : IPropertyPredicate
{
    public In(RelationEndType relationEndType = null) => this.RelationEndType = relationEndType;

    public IExtent Extent { get; set; }

    public IEnumerable<IObject> Objects { get; set; }

    public string Parameter { get; set; }

    public RelationEndType RelationEndType { get; set; }

    bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.HasMissingArguments(arguments);

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        if (!this.HasMissingArguments(arguments))
        {
            var objects = this.Parameter != null ? transaction.GetObjects(arguments.ResolveObjects(this.Parameter)) : this.Objects;

            if (this.RelationEndType is RoleType roleType)
            {
                if (objects != null)
                {
                    compositePredicate.AddIn(roleType, objects);
                }
                else
                {
                    compositePredicate.AddIn(roleType, this.Extent.Build(transaction, arguments));
                }
            }
            else
            {
                var associationType = (AssociationType)this.RelationEndType;
                if (objects != null)
                {
                    compositePredicate.AddIn(associationType, objects);
                }
                else
                {
                    compositePredicate.AddIn(associationType, this.Extent.Build(transaction, arguments));
                }
            }
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitIn(this);

    private bool HasMissingArguments(IArguments arguments) => (this.Parameter != null && arguments?.HasArgument(this.Parameter) != true) ||
                                                              this.Extent?.HasMissingArguments(arguments) == true;
}

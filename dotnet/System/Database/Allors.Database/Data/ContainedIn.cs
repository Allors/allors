// <copyright file="ContainedIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using System.Collections.Generic;
using Meta;

public class ContainedIn : IPropertyPredicate
{
    public ContainedIn(IPropertyType propertyType = null) => this.PropertyType = propertyType;

    public IExtent Extent { get; set; }

    public IEnumerable<IObject> Objects { get; set; }

    public string Parameter { get; set; }

    public IPropertyType PropertyType { get; set; }

    bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) => this.HasMissingArguments(arguments);

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        if (!this.HasMissingArguments(arguments))
        {
            var objects = this.Parameter != null ? transaction.GetObjects(arguments.ResolveObjects(this.Parameter)) : this.Objects;

            if (this.PropertyType is IRoleType roleType)
            {
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(roleType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(roleType, this.Extent.Build(transaction, arguments));
                }
            }
            else
            {
                var associationType = (IAssociationType)this.PropertyType;
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(associationType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(associationType, this.Extent.Build(transaction, arguments));
                }
            }
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitContainedIn(this);

    private bool HasMissingArguments(IArguments arguments) => (this.Parameter != null && arguments?.HasArgument(this.Parameter) != true) ||
                                                              this.Extent?.HasMissingArguments(arguments) == true;
}

﻿// <copyright file="GreaterThan.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using Allors.Database.Meta;

public class GreaterThan(RoleType roleType = null) : IRolePredicate
{
    public object Value { get; set; }

    public RoleType Path { get; set; }

    public string Parameter { get; set; }

    public RoleType RoleType { get; set; } = roleType;

    bool IPredicate.ShouldTreeShake(IArguments arguments) => ((IPredicate)this).HasMissingArguments(arguments);

    bool IPredicate.HasMissingArguments(IArguments arguments) =>
        this.Parameter != null && (arguments == null || !arguments.HasArgument(this.Parameter));

    void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
    {
        if (this.Path != null)
        {
            compositePredicate.AddGreaterThan(this.RoleType, this.Path);
        }
        else
        {
            var value = this.Parameter != null ? arguments.ResolveUnit(this.RoleType.ObjectType.Tag, this.Parameter) : this.Value;
            compositePredicate.AddGreaterThan(this.RoleType, value);
        }
    }

    public void Accept(IVisitor visitor) => visitor.VisitGreaterThan(this);
}

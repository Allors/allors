// <copyright file="ITrace.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Tracing;

using System.Text;
using Allors.Database.Adapters.Tracing;
using Allors.Database.Meta;

public sealed class SqlAddCompositeRoleEvent : Event
{
    public SqlAddCompositeRoleEvent(ITransaction transaction) : base(transaction)
    {
    }

    public RoleType RoleType { get; set; }

    public CompositeRelation[] Relations { get; set; }

    protected override void ToString(StringBuilder builder) => _ = builder
        .Append('[')
        .Append(this.RoleType.Name)
        .Append(" -> #")
        .Append(this.Relations.Length)
        .Append("] ");
}

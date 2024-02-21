// <copyright file="ITrace.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Tracing;

using System.Linq;
using System.Text;
using Allors.Database.Adapters.Tracing;
using Allors.Database.Meta;

public sealed class SqlSetUnitRolesEvent : Event
{
    public SqlSetUnitRolesEvent(ITransaction transaction) : base(transaction)
    {
    }

    public Strategy Strategy { get; set; }

    public RoleType[] RoleTypes { get; set; }

    protected override void ToString(StringBuilder builder) => _ = builder
        .Append('[')
        .Append(this.Strategy.ObjectId)
        .Append(" : ")
        .Append(string.Join(", ", this.RoleTypes.Select(v => v.Name)))
        .Append("] ");
}

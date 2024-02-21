// <copyright file="ITrace.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Tracing;

using System.Text;
using Allors.Database.Adapters.Tracing;
using Allors.Database.Meta;

public sealed class SqlPrefetchUnitRolesEvent : Event
{
    public SqlPrefetchUnitRolesEvent(ITransaction transaction) : base(transaction)
    {
    }

    public Reference[] Associations { get; set; }

    public Class Class { get; set; }

    public RoleType RoleType { get; set; }

    protected override void ToString(StringBuilder builder) => _ = builder
        .Append('[')
        .Append(this.Class.SingularName)
        .Append(" : ")
        .Append(this.RoleType.Name)
        .Append(" -> #")
        .Append(this.Associations.Length)
        .Append("] ");
}

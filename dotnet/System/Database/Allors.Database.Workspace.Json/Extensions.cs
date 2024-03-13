// <copyright file="FromJsonVisitor.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json;

using System.Linq;
using Allors.Protocol.Json;
using Allors.Database.Data;
using Allors.Database.Meta;
using Extent = Allors.Protocol.Json.Data.Extent;

public static class Extensions
{
    public static Composite FindComposite(this MetaPopulation @this, string tag) => tag != null ? (Composite)@this.FindByTag(tag) : null;

    public static AssociationType FindAssociationType(this MetaPopulation @this, string tag) =>
        tag != null ? ((RoleType)@this.FindByTag(tag)).AssociationType : null;

    public static RoleType FindRoleType(this MetaPopulation @this, string tag) =>
        tag != null ? ((RoleType)@this.FindByTag(tag)) : null;

    public static Pull[] FromJson(this Allors.Protocol.Json.Data.Pull[] pulls, ITransaction transaction, IUnitConvert unitConvert)
    {
        var fromJson = new FromJson(transaction, unitConvert);

        try
        {
            return pulls.Select(v =>
            {
                var fromJsonVisitor = new FromJsonVisitor(fromJson);
                v.Accept(fromJsonVisitor);
                return fromJsonVisitor.Pull;
            }).ToArray();
        }
        finally
        {
            fromJson.Resolve();
        }
    }

    public static IExtent FromJson(this Extent extent, ITransaction transaction, IUnitConvert unitConvert)
    {
        var fromJson = new FromJson(transaction, unitConvert);
        var fromJsonVisitor = new FromJsonVisitor(fromJson);
        extent.Accept(fromJsonVisitor);
        fromJson.Resolve();
        return fromJsonVisitor.Extent;
    }

    public static Select FromJson(this Allors.Protocol.Json.Data.Select select, ITransaction transaction, IUnitConvert unitConvert)
    {
        var fromJson = new FromJson(transaction, unitConvert);
        var fromJsonVisitor = new FromJsonVisitor(fromJson);
        select.Accept(fromJsonVisitor);
        fromJson.Resolve();
        return fromJsonVisitor.Select;
    }

    public static Allors.Protocol.Json.Data.Pull ToJson(this Pull pull, IUnitConvert unitConvert)
    {
        var toJsonVisitor = new ToJsonVisitor(unitConvert);
        pull.Accept(toJsonVisitor);
        return toJsonVisitor.Pull;
    }

    public static Extent ToJson(this IExtent extent, IUnitConvert unitConvert)
    {
        var toJsonVisitor = new ToJsonVisitor(unitConvert);
        extent.Accept(toJsonVisitor);
        return toJsonVisitor.Extent;
    }

    public static Allors.Protocol.Json.Data.Select ToJson(this Select extent, IUnitConvert unitConvert)
    {
        var toJsonVisitor = new ToJsonVisitor(unitConvert);
        extent.Accept(toJsonVisitor);
        return toJsonVisitor.Select;
    }
}

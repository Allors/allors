namespace Allors.Database.Meta.Extensions;

using System.Collections.Generic;
using System;
using System.Collections.Concurrent;

public static class IClassExtensions
{
    public static long CreatePermissionId(this IClass @this)
    {
        return @this.Extensions.CreatePermissionId ?? false;
    }

    public static void CreatePermissionId(this IClass @this, long value)
    {
        @this.Extensions.CreatePermissionId = value;
    }

    public static IReadOnlyDictionary<Guid, long> ReadPermissionIdByRelationTypeId(this IClass @this)
    {
        return @this.Extensions.ReadPermissionIdByRelationTypeId;
    }

    public static void ReadPermissionIdByRelationTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Extensions.ReadPermissionIdByRelationTypeId = value;
    }

    public static IReadOnlyDictionary<Guid, long> WritePermissionIdByRelationTypeId(this IClass @this)
    {
        return @this.Extensions.WritePermissionIdByRelationTypeId ??= new Dictionary<Guid, long>();
    }

    public static void WritePermissionIdByRelationTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Extensions.WritePermissionIdByRelationTypeId = value;
    }

    public static IReadOnlyDictionary<Guid, long> ExecutePermissionIdByMethodTypeId(this IClass @this)
    {
        return @this.Extensions.ExecutePermissionIdByMethodTypeId ??= new Dictionary<Guid, long>();
    }

    public static void ExecutePermissionIdByMethodTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Extensions.ExecutePermissionIdByMethodTypeId = value;
    }

}

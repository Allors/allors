namespace Allors.Database.Meta.Extensions;

using System.Collections.Generic;
using System;

public static class IClassExtensions
{
    public static long CreatePermissionId(this IClass @this)
    {
        return @this.Attributes.CreatePermissionId ?? false;
    }

    public static void CreatePermissionId(this IClass @this, long value)
    {
        @this.Attributes.CreatePermissionId = value;
    }

    public static IReadOnlyDictionary<Guid, long> ReadPermissionIdByRelationTypeId(this IClass @this)
    {
        return @this.Attributes.ReadPermissionIdByRelationTypeId;
    }

    public static void ReadPermissionIdByRelationTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Attributes.ReadPermissionIdByRelationTypeId = value;
    }

    public static IReadOnlyDictionary<Guid, long> WritePermissionIdByRelationTypeId(this IClass @this)
    {
        return @this.Attributes.WritePermissionIdByRelationTypeId ??= new Dictionary<Guid, long>();
    }

    public static void WritePermissionIdByRelationTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Attributes.WritePermissionIdByRelationTypeId = value;
    }

    public static IReadOnlyDictionary<Guid, long> ExecutePermissionIdByMethodTypeId(this IClass @this)
    {
        return @this.Attributes.ExecutePermissionIdByMethodTypeId ??= new Dictionary<Guid, long>();
    }

    public static void ExecutePermissionIdByMethodTypeId(this IClass @this, IReadOnlyDictionary<Guid, long> value)
    {
        @this.Attributes.ExecutePermissionIdByMethodTypeId = value;
    }

}

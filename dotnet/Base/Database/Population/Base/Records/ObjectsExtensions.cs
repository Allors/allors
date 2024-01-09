namespace Allors.Database.Population;

using System;
using System.Collections.Generic;
using System.Linq;
using Meta;
using Meta.Extensions;
using Population;

public static class ObjectsExtensions
{
    public static IDictionary<IClass, Record[]> ToRecordsByClass(this IEnumerable<IObject> objects, Func<IStrategy, Handle> handleResolver)
    {

        var recordsByClass = objects
            .Select(v => v.Strategy)
            .Select(v =>
            {
                var handle = handleResolver(v);

                var valueByRoleType = v.Class.RoleTypes
                    .Where(roleType => roleType.ObjectType.IsUnit &&
                                       !roleType.RelationType.IsDerived &&
                                       v.ExistRole(roleType))
                    .ToDictionary(roleType => roleType, v.GetUnitRole);

                return new Record(v.Class, handle, valueByRoleType);
            })
            .GroupBy(v => v.Class)
            .ToDictionary(v => v.Key, v => v.ToArray());

        return recordsByClass;
    }
}

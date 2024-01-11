namespace Allors.Database.Population;

using System;
using System.Collections.Generic;
using System.Linq;
using Meta;

public static class ObjectsExtensions
{
    public static IDictionary<IClass, Record[]> ToRecordsByClass(this IEnumerable<IObject> objects, IRecordRoundtripStrategy recordRoundtripStrategy)
    {
        var handleResolver = recordRoundtripStrategy.HandleResolver();
        var roleFilter = recordRoundtripStrategy.RoleFilter();

        var recordsByClass = objects
            .Select(strategy => strategy.Strategy)
            .Select(strategy =>
            {
                var handle = handleResolver(strategy);

                var valueByRoleType = strategy.Class.RoleTypes
                    .Where(roleType => roleFilter(strategy, roleType))
                    .ToDictionary(roleType => roleType, strategy.GetUnitRole);

                return new Record(strategy.Class, handle, valueByRoleType);
            })
            .GroupBy(record => record.Class)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());

        return recordsByClass;
    }
}

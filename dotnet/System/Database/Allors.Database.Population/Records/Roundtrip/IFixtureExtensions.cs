namespace Allors.Database.Roundtrip;

using System.Collections.Generic;
using System.Linq;
using Population;
using Meta;

public static class IFixtureExtensions
{
    public static void ToDatabase(this IDictionary<IClass, Record[]> recordsByClass, ITransaction transaction)
    {
        Dictionary<IClass, Dictionary<object, IObject>> objectByKeyByClass = new();

        foreach (var kvp in recordsByClass)
        {
            var @class = kvp.Key;
            var records = kvp.Value;
            var keyRoleType = @class.KeyRoleType;

            var objectByKey = transaction.Extent(@class).ToDictionary(v => v.Strategy.GetUnitRole(keyRoleType));
            objectByKeyByClass.Add(@class, objectByKey);

            foreach (var record in records)
            {
                var key = record.ValueByRoleType[@class.KeyRoleType];

                if (!objectByKey.TryGetValue(key, out var @object))
                {
                    @object = transaction.Build(@class, v =>
                    {
                        var strategy = v.Strategy;
                        foreach ((IRoleType roleType, object value) in record.ValueByRoleType.Where(role => role.Key.ObjectType.IsUnit))
                        {
                            strategy.SetRole(roleType, value);
                        }
                    });

                    objectByKey.Add(key, @object);
                }
            }
        }
    }
}

namespace Allors.Database.Population;

using System;
using System.Collections.Generic;
using System.Linq;
using CaseExtensions;
using Meta;
using Population;

public static class HandleResolvers
{
    public static Func<IStrategy, Handle> FromExisting(IDictionary<IClass, Record[]> existingRecordsByClass)
    {
        var handleByKeyByClass = existingRecordsByClass
            .SelectMany(v => v.Value)
            .Where(v => v.Handle != null)
            .GroupBy(v => v.Class)
            .ToDictionary(v => v.Key, v => v.ToDictionary(w => w.ValueByRoleType[w.Class.KeyRoleType], w => w.Handle));

        return strategy =>
        {
            if (!handleByKeyByClass.TryGetValue(strategy.Class, out var handleByKey))
            {
                return null;
            }

            var key = strategy.GetUnitRole(strategy.Class.KeyRoleType);
            if (key == null)
            {
                return null;
            }

            handleByKey.TryGetValue(key, out var handle);
            return handle;
        };
    }

    public static Func<IStrategy, Handle> PascalCaseKey()
    {
        return strategy =>
        {
            var keyRoleType = strategy.Class.KeyRoleType;
            var keyValue = strategy.GetUnitRole(keyRoleType);
            if (keyValue is not string keyString)
            {
                return null;
            }

            var keyName = keyString.ToPascalCase();

            return new Handle(keyRoleType, keyName, keyString);
        };
    }
}

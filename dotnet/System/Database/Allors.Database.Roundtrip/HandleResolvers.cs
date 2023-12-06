namespace Allors.Database.Roundtrip;

using System;
using System.Linq;
using CaseExtensions;
using Fixture;

public static class HandleResolvers
{
    public static Func<IStrategy, Handle> FromFixture(Fixture existingFixture)
    {
        var handleByKeyByClass = existingFixture.RecordsByClass
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
            if (keyValue is not string)
            {
                return null;
            }

            var keyName = ((string)keyValue).ToPascalCase();

            return new Handle(keyRoleType, keyName, keyValue);
        };
    }
}

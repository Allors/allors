namespace Allors.Database.Roundtrip;

using System;
using System.Linq;
using Fixture;

public static class ITransactionExtensions
{
    public static Fixture ToFixture(this ITransaction transaction, Func<IStrategy, Handle> handleResolver)
    {
        var metaPopulation = transaction.Database.MetaPopulation;

        bool IsDefault(object value)
        {
            return value switch
            {
                bool booleanValue => booleanValue == false,
                decimal decimalValue => decimalValue == 0m,
                double doubleValue => doubleValue == 0d,
                int intValue => intValue == 0,
                _ => false
            };
        }

        var recordsByClass = metaPopulation.Classes
            .Where(v => v.KeyRoleType != null)
            .SelectMany(transaction.Extent)
            .Select(v => v.Strategy)
            .Select(v =>
            {
                var handle = handleResolver(v);

                var valueByRoleType = v.Class.RoleTypes
                    .Where(roleType => roleType.ObjectType.IsUnit &&
                                       !roleType.RelationType.IsDerived &&
                                       v.ExistRole(roleType) &&
                                       !IsDefault(v.GetUnitRole(roleType)))
                    .ToDictionary(roleType => roleType, v.GetUnitRole);

                return new Record(v.Class, handle, valueByRoleType);
            })
            .GroupBy(v => v.Class)
            .ToDictionary(v => v.Key, v => v.ToArray());

        return new Fixture(recordsByClass);
    }
}

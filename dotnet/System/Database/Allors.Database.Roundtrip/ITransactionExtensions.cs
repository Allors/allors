namespace Allors.Database.Roundtrip;

using System.Linq;
using Database.Population;
using Meta;

public static class ITransactionExtensions
{
    public static Fixture ToFixture(this ITransaction transaction)
    {
        var metaPopulation = transaction.Database.MetaPopulation;

        var recordsByClass = metaPopulation.Classes
            .Where(v => v.KeyRoleType != null)
            .SelectMany(v => transaction.Extent(v).ToArray())
            .Select(v =>
            {
                var valueByRoleType = v.Strategy.Class.RoleTypes
                    .Where(roleType => roleType.ObjectType.IsUnit && !roleType.RelationType.IsDerived &&
                                       v.Strategy.ExistRole(roleType))
                    .ToDictionary(w => w, w => v.Strategy.GetUnitRole(w));
                return new Record(v.Strategy.Class, null, valueByRoleType);
            })
            .GroupBy(v => v.Class)
            .ToDictionary(v => v.Key, v => v.ToArray());

        return new Fixture(recordsByClass);
    }
}

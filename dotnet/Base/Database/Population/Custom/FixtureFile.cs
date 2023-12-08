namespace Allors.Population
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Meta;
    using Database.Population;
    using Database.Roundtrip;

    public partial class FixtureFile
    {
        private Func<IStrategy, Handle> HandleResolver()
        {
            var excluded = new HashSet<IClass> { this.m.Country, this.m.Currency, this.m.Language };

            var fromExisting = HandleResolvers.FromFixture(this.ExistingRecordsByClass);
            var fromKey = HandleResolvers.PascalCaseKey();

            return strategy =>
            {
                if (excluded.Contains(strategy.Class))
                {
                    return null;
                }

                return fromExisting(strategy) ?? fromKey(strategy);
            };
        }

        private IEnumerable<IObject> Objects(ITransaction transaction)
        {
            var classes = new IClass[] { this.m.Country, this.m.Currency, this.m.Language };

            return classes
                .Union(this.ExistingRecordsByClass.Keys)
                .Distinct()
                .SelectMany(transaction.Extent);
        }
    }
}

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
            var excluded = new HashSet<IClass> { this.m.Country };
            var fromExisting = HandleResolvers.FromFixture(this.ExistingFixture);
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
            var extraClasses = new IClass[] { this.m.Country };
            var existingClasses = this.ExistingFixture.RecordsByClass.Keys;
            var classes = extraClasses.Union(existingClasses).Distinct();

            return classes
                .SelectMany(transaction.Extent);
        }
    }
}

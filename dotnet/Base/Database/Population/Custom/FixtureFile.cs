namespace Allors.Population
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Population;
    using Database.Roundtrip;

    public partial class FixtureFile
    {
        private Func<IStrategy, Handle> HandleResolver()
        {
            Func<IStrategy, Handle> handleResolver = _ => null;

            var fromExisting = HandleResolvers.FromFixture(this.ExistingFixture);
            var fromKey = HandleResolvers.PascalCaseKey();
            handleResolver = strategy => fromExisting(strategy) ?? fromKey(strategy);
            return handleResolver;
        }

        private IEnumerable<IObject> Objects(ITransaction transaction)
        {
            return this.ExistingFixture.RecordsByClass.Keys
                .SelectMany(transaction.Extent);
        }
    }
}

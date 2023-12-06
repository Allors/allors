namespace Allors.Fixture
{
    using System;
    using Database;
    using Database.Fixture;
    using Database.Roundtrip;

    public partial class FixtureFile
    {
        private Func<IStrategy, Handle> HandleResolver()
        {
            Func<IStrategy, Handle> handleResolver = _ => null;
            Fixture existingFixture = this.ExistingFixture();

            var fromExisting = HandleResolvers.FromFixture(existingFixture);
            var fromKey = HandleResolvers.PascalCaseKey();
            handleResolver = strategy => fromExisting(strategy) ?? fromKey(strategy);
            return handleResolver;
        }
    }
}

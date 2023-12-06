namespace Allors.Fixture
{
    using System;
    using System.IO;
    using Database;
    using Database.Fixture;
    using Database.Fixture.Xml;
    using Database.Meta;
    using Database.Roundtrip;

    public partial class FixtureFile
    {
        private readonly IDatabase database;
        private readonly FileInfo fileInfo;
        private readonly M m;

        public FixtureFile(IDatabase database, FileInfo fileInfo)
        {
            this.database = database;
            this.fileInfo = fileInfo;
            this.m = database.Services.Get<M>();
        }

        public void Write()
        {
            using var transaction = this.database.CreateTransaction();

            Func<IStrategy, Handle> handleResolver = this.HandleResolver();

            using var stream = File.Open(fileInfo.FullName, FileMode.Create);
            var fixture = transaction.ToFixture(handleResolver);
            var fixtureWriter = new FixtureWriter(m);
            fixtureWriter.Write(stream, fixture);
        }
        
        private Fixture ExistingFixture()
        {
            if (!this.fileInfo.Exists)
            {
                return null;
            }

            using var existingStream = File.Open(this.fileInfo.FullName, FileMode.Open);
            var fixtureReader = new FixtureReader(this.m);
            var existingFixture = fixtureReader.Read(existingStream);
            return existingFixture;
        }
    }
}

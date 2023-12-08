namespace Allors.Workspace.WinForms.ViewModels.Tests
{
    using Allors.Database;
    using Allors.Database.Adapters.Memory;
    using Allors.Database.Configuration;
    using Allors.Database.Domain;
    using Allors.Database.Services;
    using Allors.Workspace;
    using Allors.Workspace.Meta.Static;
    using Configuration;
    using Database.Meta;
    using Database.Population;
    using Population;
    using Configuration = Allors.Workspace.Adapters.Direct.Configuration;
    using Connection = Allors.Workspace.Adapters.Direct.Connection;
    using Person = Allors.Workspace.Domain.Person;

    [TestFixture]
    public abstract class Test
    {
        private Fixture fixture;

        private Func<IWorkspaceServices> servicesBuilder;
        private Configuration configuration;

        public Database Database { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.fixture = new Fixture();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.fixture?.Dispose();
        }

        [SetUp]
        public void TestSetup()
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Person));

            this.servicesBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
            this.configuration = new Configuration("Default", metaPopulation);

            this.Database = new Database(
                new DefaultDatabaseServices(this.fixture.Engine),
                new Allors.Database.Adapters.Memory.Configuration
                {
                    ObjectFactory = new ObjectFactory(this.fixture.M, typeof(Allors.Database.Domain.Person)),
                });

            this.Database.Init();

            var config = new Config();
            var recordsFromResource = new RecordsFromResource(typeof(RoundtripStrategy).Assembly, this.Database.MetaPopulation);
            new Setup(this.Database, recordsFromResource.RecordsByClass, config).Apply();

            using var transaction = this.Database.CreateTransaction();

            var administrator = transaction.Build<Allors.Database.Domain.Person>(v => v.UserName = "administrator");
            transaction.Scoped<UserGroupByUniqueId>().Administrators.AddMember(administrator);
            transaction.Services.Get<IUserService>().User = administrator;

            transaction.Derive();
            transaction.Commit();
        }

        public Connection Connect(string userName)
        {
            using var transaction = this.Database.CreateTransaction();
            var user = transaction.Extent<User>().ToArray().First(v => v.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            transaction.Services.Get<IUserService>().User = user;
            return new Connection(this.configuration, this.Database, this.servicesBuilder) { UserId = user.Id };
        }
    }
}

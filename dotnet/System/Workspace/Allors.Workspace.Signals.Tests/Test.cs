namespace Allors.Workspace.Signals.Tests
{
    using Adapters.Direct;
    using Database;
    using Database.Adapters.Memory;
    using Database.Configuration;
    using Database.Configuration.Derivations.Default;
    using Population;
    using Configuration = Database.Adapters.Memory.Configuration;

    public class Test
    {

        private Database database;

        public Adapters.Direct.Configuration configuration;

        private Func<WorkspaceServices> servicesBuilder;

        private Allors.Database.Security.IUser user;

        public ITransaction Transaction { get; private set; }
        public Connection Connection { get; set; }

        public IWorkspace Workspace { get; set; }

        [SetUp]
        public void TestSetup()
        {
            {
                var metaBuilder = new Allors.Database.Meta.Configuration.MetaBuilder();
                var metaPopulation = metaBuilder.Build();
                var rules = Allors.Database.Domain.Rules.Create(metaPopulation);
                var engine = new Engine(rules);
                this.database = new Database(
                    new DefaultDatabaseServices(engine),
                    new Configuration
                    {
                        ObjectFactory = new ObjectFactory(metaPopulation, typeof(Allors.Database.Domain.Person)),
                    });

                this.database.Init();
                var config = new Allors.Database.Domain.Config();
                var recordsFromResource = new RecordsFromResource(typeof(RoundtripStrategy).Assembly, metaPopulation);
                var recordsByClass = recordsFromResource.Read();
                new Allors.Database.Domain.Setup(this.database, recordsByClass, config).Apply();
                this.Transaction = this.database.CreateTransaction();
                new Allors.Database.Domain.TestPopulation(this.Transaction).Apply();
                this.Transaction.Commit();
            }

            {
                var metaPopulation = new Meta.Static.MetaBuilder().Build();
                var objectFactory = new Allors.Workspace.Configuration.ReflectionObjectFactory(metaPopulation, typeof(Domain.Person));
                this.servicesBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
                this.configuration = new Adapters.Direct.Configuration("Default", metaPopulation);
            }
        }

        [TearDown]
        public void TestTearDown()
        {
            this.Transaction.Dispose();
        }

        public IWorkspace CreateExclusiveWorkspace()
        {
            var connection = new Connection(this.configuration, this.database, this.servicesBuilder) { UserId = this.user.Id };
            return connection.CreateWorkspace();
        }

        public IWorkspace CreateSharedWorkspace() => this.Connection.CreateWorkspace();

        public Task Login(string userName)
        {
            this.user = this.Transaction.Extent<Allors.Database.Domain.User>().First(v => v.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            this.Transaction.Services.Get<Allors.Database.Services.IUserService>().User = this.user;

            this.Connection = new Connection(this.configuration, this.database, this.servicesBuilder) { UserId = this.user.Id };

            this.Workspace = this.Connection.CreateWorkspace();

            return Task.CompletedTask;
        }
    }
}

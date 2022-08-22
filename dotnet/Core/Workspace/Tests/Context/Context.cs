namespace Tests.Workspace
{
    using Allors.Workspace;

    public abstract class Context
    {
        protected Context(Test test, string name)
        {
            this.Test = test;
            this.Name = name;
            this.SharedDatabaseWorkspace = this.Test.Profile.CreateWorkspaceConnection();
            this.SharedDatabaseSession = this.SharedDatabaseWorkspace.CreateWorkspace();
            this.ExclusiveDatabaseWorkspace = this.Test.Profile.CreateExclusiveWorkspaceConnection();
            this.ExclusiveDatabaseSession = this.ExclusiveDatabaseWorkspace.CreateWorkspace();
        }

        public Test Test { get; }

        public string Name { get; }

        public IWorkspace Session1 { get; protected set; }

        public IWorkspace Session2 { get; protected set; }

        public IWorkspaceConnection SharedDatabaseWorkspace { get; }

        public IWorkspace SharedDatabaseSession { get; }

        public IWorkspaceConnection ExclusiveDatabaseWorkspace { get; }

        public IWorkspace ExclusiveDatabaseSession { get; }

        public void Deconstruct(out IWorkspace session1, out IWorkspace session2)
        {
            session1 = this.Session1;
            session2 = this.Session2;
        }

        public override string ToString() => this.Name;
    }
}

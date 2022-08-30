namespace Tests.Workspace
{
    using Allors.Workspace;

    public class Context
    {
        public Context(Test test)
        {
            this.Test = test;
            this.SharedDatabase = this.Test.Profile.CreateWorkspaceConnection();
            this.ExclusiveDatabase = this.Test.Profile.CreateExclusiveWorkspaceConnection();
        }

        public Test Test { get; }

        public IConnection SharedDatabase { get; }

        public IConnection ExclusiveDatabase { get; }

        public void Deconstruct(out IConnection connection1, out IConnection connection2)
        {
            connection1 = this.SharedDatabase;
            connection2 = this.ExclusiveDatabase;
        }
    }
}

namespace Allors.Workspace.Adapters.Tests
{
    public class SingleSessionContext : Context
    {
        public SingleSessionContext(Test test, string name) : base(test, name)
        {
            this.Workspace1 = this.Test.Profile.CreateWorkspace();
            this.Workspace2 = this.Workspace1;
        }
    }
}

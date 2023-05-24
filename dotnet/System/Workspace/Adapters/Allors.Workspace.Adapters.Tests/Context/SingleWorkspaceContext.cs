namespace Allors.Workspace.Adapters.Tests
{
    public class SingleWorkspaceContext : Context
    {
        public SingleWorkspaceContext(Test test, string name) : base(test, name)
        {
            this.Workspace1 = this.Test.Profile.CreateSharedWorkspace();
            this.Workspace2 = this.Workspace1;
        }
    }
}

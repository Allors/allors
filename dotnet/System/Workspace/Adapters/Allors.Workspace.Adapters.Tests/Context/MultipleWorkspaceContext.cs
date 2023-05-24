namespace Allors.Workspace.Adapters.Tests
{
    public class MultipleWorkspaceContext : Context
    {
        public MultipleWorkspaceContext(Test test, string name) : base(test, name)
        {
            this.Workspace1 = this.Test.Profile.CreateSharedWorkspace();
            this.Workspace2 = this.Test.Profile.CreateExclusiveWorkspace();
        }
    }
}

namespace Allors.Workspace.Adapters.Tests
{
    public class MultipleSessionContext : Context
    {
        public MultipleSessionContext(Test test, string name) : base(test, name)
        {
            this.Workspace1 = this.Test.Profile.CreateWorkspace();
            this.Workspace2 = this.Test.Profile.CreateExclusiveWorkspace();
        }
    }
}

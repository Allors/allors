namespace Allors.Workspace.Adapters.Tests
{
    public class MultipleSessionContext : Context
    {
        public MultipleSessionContext(Test test, string name) : base(test, name)
        {
            this.Session1 = this.Test.Profile.CreateWorkspace();
            this.Session2 = this.Test.Profile.CreateExclusiveWorkspace();
        }
    }
}

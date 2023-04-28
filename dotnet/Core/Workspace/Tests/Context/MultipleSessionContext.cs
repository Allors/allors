namespace Allors.Workspace.Adapters.Tests
{
    public class MultipleSessionContext : Context
    {
        public MultipleSessionContext(Test test, string name) : base(test, name)
        {
            this.Session1 = test.Workspace.CreateSession();
            this.Session2 = test.Workspace.CreateSession();
        }
    }
}

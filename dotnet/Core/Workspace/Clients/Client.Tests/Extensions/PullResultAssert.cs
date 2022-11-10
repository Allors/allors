namespace Tests.Workspace
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;

    public class PullResultAssert
    {
        private readonly IPullResult pullResult;

        public PullResultAssert(IPullResult pullResult) => this.pullResult = pullResult;

        public PullResultCollectionAssert Collection(IComposite objectType) => new PullResultCollectionAssert(this.pullResult, objectType);

        public PullResultCollectionAssert Collection(string name) => new PullResultCollectionAssert(this.pullResult, name);
    }
}

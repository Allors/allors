namespace Tests.Workspace
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Xunit;

    public class PullResultCollectionAssert
    {
        private readonly IObject[] collection;

        public PullResultCollectionAssert(IPullResult pullResult, IComposite objectType) => pullResult.GetCollection(objectType);

        public PullResultCollectionAssert(IPullResult pullResult, string name) => pullResult.GetCollection(name);

        public void Single() => Assert.Single(this.collection);

        public void Equal(params string[] expected)
        {
            var actual = this.collection.Select(v => (string)((dynamic)v).Name);
            Assert.Equal(expected, actual);
        }
    }
}

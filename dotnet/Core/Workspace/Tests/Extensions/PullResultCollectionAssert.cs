namespace Tests.Workspace
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Xunit;

    public class PullResultCollectionAssert
    {
        private readonly IObject[] collection;

        public PullResultCollectionAssert(IPullResult pullResult, IComposite objectType) => this.collection = pullResult.GetCollection(objectType);

        public PullResultCollectionAssert(IPullResult pullResult, string name) => this.collection = pullResult.GetCollection(name);

        public void Equal(params string[] expected)
        {
            var actual = this.collection.Select(v =>
            {
                var roleType = v.Class.RoleTypes.First(v => v.SingularName == "Name");
                return v.GetUnitRole(roleType);
            });
            Assert.Equal(expected, actual);
        }
    }
}

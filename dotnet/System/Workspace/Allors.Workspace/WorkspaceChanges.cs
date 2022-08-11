namespace Allors.Workspace
{
    using Ranges;

    public class WorkspaceChanges
    {
        public WorkspaceChanges(IRange<long> objectIds) => this.ObjectIds = objectIds;

        private IRange<long> ObjectIds { get; }
    }
}

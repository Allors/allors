namespace Allors.Workspace
{
    using Shared.Ranges;

    public class WorkspaceChanges
    {
        public WorkspaceChanges(ValueRange<long> objectIds) => this.ObjectIds = objectIds;

        private ValueRange<long> ObjectIds { get; }
    }
}

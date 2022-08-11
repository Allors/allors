namespace Allors.Workspace
{
    using System.Collections.Generic;

    public class WorkspaceChanges
    {
        public WorkspaceChanges(ISet<long> objectIds) => this.ObjectIds = objectIds;

        private ISet<long> ObjectIds { get; }
    }
}

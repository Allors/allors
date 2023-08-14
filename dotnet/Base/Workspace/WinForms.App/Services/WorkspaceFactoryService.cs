namespace Workspace.WinForms.App.Services
{
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Mvvm.Services;
    using ViewModels.Services;

    public class WorkspaceFactoryService : IWorkspaceFactoryService
    {
        public WorkspaceFactoryService(Connection connection) => this.Connection = connection;

        public Connection Connection { get; }

        public IWorkspace CreateWorkspace() => this.Connection.CreateWorkspace();
    }
}

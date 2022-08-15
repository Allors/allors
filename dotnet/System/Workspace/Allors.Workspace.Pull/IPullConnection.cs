namespace Allors.Workspace.State
{
    using System.Threading.Tasks;
    using Allors.Workspace.Pull.Request;

    public interface IPullConnection
    {
        Task<IPullResponse> Pull(IPullRequest request);
    }
}

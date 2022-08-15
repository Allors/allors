namespace Allors.Workspace.State
{
    using System.Threading.Tasks;

    public interface IPullConnection
    {
        Task<IPullResponse> Pull(IPullRequest request);
    }
}

namespace Allors.Workspace.State
{
    using System.Collections.Generic;

    public interface IState
    {
        IDictionary<long, IObjectState> ObjectStateById { get; }
    }
}

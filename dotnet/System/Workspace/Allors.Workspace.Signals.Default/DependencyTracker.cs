namespace Allors.Workspace.Signals.Default;

using System.Collections.Generic;

public class DependencyTracker : IDependencyTracker
{
    public Dictionary<IOperand, long> WorkspaceVersionByOperand { get; } = new();

    public void Track(IOperand operand)
    {
        if (!this.WorkspaceVersionByOperand.ContainsKey(operand))
        {
            this.WorkspaceVersionByOperand.Add(operand, operand.WorkspaceVersion);
        }
    }
}

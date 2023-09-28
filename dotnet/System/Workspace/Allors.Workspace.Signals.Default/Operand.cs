namespace Allors.Workspace.Signals.Default;

using System;

public class Operand : IDownstream
{
    private readonly IOperand operand;

    public Operand(IOperand operand)
    {
        this.operand = operand;
    }

    public WeakReference<IUpstream> Upstreams { get; set; }
}

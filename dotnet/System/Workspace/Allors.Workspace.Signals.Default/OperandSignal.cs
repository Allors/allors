namespace Allors.Workspace.Signals.Default;

using System;

public class OperandSignal : IDownstream
{
    private readonly IOperand operand;

    public OperandSignal(IOperand operand)
    {
        this.operand = operand;
    }

    public WeakReference<IUpstream>[] Upstreams { get; set; }

    public void TrackedBy(IUpstream newUpstream)
    {
        this.Upstreams = this.Upstreams.Update(newUpstream);
    }

    public void OnChanged()
    {
        var upstreams = this.Upstreams;
        foreach (var weakReference in upstreams)
        {
            weakReference.TryGetTarget(out var upstream);
            upstream?.Invalidate();
        }
    }
}

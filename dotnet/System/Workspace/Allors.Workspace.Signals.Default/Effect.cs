namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class Effect : IEffect, IUpstream
{
    private static readonly IDictionary<IOperand, long> EmptyDictionary = new Dictionary<IOperand, long>();

    private readonly Dispatcher dispatcher;
    private readonly Action<ITracker> dependencies;

    private IDictionary<IOperand, long> workspaceVersionByOperand;
    private IDictionary<IOperand, long> trackingWorkspaceVersionByOperand;

    private bool shouldRaise;

    public Effect(Dispatcher dispatcher, Action<ITracker> dependencies, Action action)
    {
        this.dispatcher = dispatcher;
        this.dependencies = dependencies;
        this.Action = action;

        this.workspaceVersionByOperand = EmptyDictionary;
    }

    public Action Action { get; }

    public void Dispose()
    {
        this.dispatcher.RemoveEffect(this);
    }

    public void Raise()
    {
        this.trackingWorkspaceVersionByOperand = new Dictionary<IOperand, long>();
        this.dependencies(this);

        if (this.shouldRaise)
        {
            this.shouldRaise = false;
            this.Action();
        }

        this.workspaceVersionByOperand = this.trackingWorkspaceVersionByOperand;
        this.trackingWorkspaceVersionByOperand = null;
    }

    public void Track(IOperand operand)
    {
        if (operand == null || this.trackingWorkspaceVersionByOperand.ContainsKey(operand))
        {
            return;
        }

        var trackingWorkspaceVersion = operand.WorkspaceVersion;
        if (this.workspaceVersionByOperand.TryGetValue(operand, out var workspaceVersion))
        {
            if (workspaceVersion != trackingWorkspaceVersion)
            {
                this.shouldRaise = true;
            }
        }
        else
        {
            this.shouldRaise = true;
        }

        this.trackingWorkspaceVersionByOperand[operand] = trackingWorkspaceVersion;
    }

    public void Invalidate()
    {
        this.dispatcher.Schedule(this);
    }
}

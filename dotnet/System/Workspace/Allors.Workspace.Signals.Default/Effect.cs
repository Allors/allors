namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class Effect : IEffect, IUpstream
{
    private static readonly IDictionary<ISignal, long> EmptyDictionary = new Dictionary<ISignal, long>();

    private readonly Dispatcher dispatcher;
    private readonly Action<ITracker> dependencies;

    private IDictionary<ISignal, long> workspaceVersionBySignal;
    private IDictionary<ISignal, long> trackingWorkspaceVersionBySignal;

    private bool shouldRaise;
    
    public Effect(Dispatcher dispatcher, Action<ITracker> dependencies, Action action)
    {
        this.dispatcher = dispatcher;
        this.dependencies = dependencies;
        this.Action = action;

        this.workspaceVersionBySignal = EmptyDictionary;
    }

    public Action Action { get; }

    public void Dispose()
    {
        this.dispatcher.RemoveEffect(this);
    }

    public void Invalidate()
    {
        // TODO:
    }
    
    public void Handle()
    {
        this.trackingWorkspaceVersionBySignal = new Dictionary<ISignal, long>();
        this.dependencies(this);

        if (this.shouldRaise)
        {
            this.shouldRaise = false;
            this.Action();
        }

        this.workspaceVersionBySignal = this.trackingWorkspaceVersionBySignal;
        this.trackingWorkspaceVersionBySignal = null;
        
        this.dispatcher.UpdateTracked(this, this.workspaceVersionBySignal.Keys);
    }

    public void Track(ISignal signal)
    {
        if (signal == null || this.trackingWorkspaceVersionBySignal.ContainsKey(signal))
        {
            return;
        }
        
        var trackingWorkspaceVersion = signal.Version;
        if (this.workspaceVersionBySignal.TryGetValue(signal, out var workspaceVersion))
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

        this.trackingWorkspaceVersionBySignal[signal] = trackingWorkspaceVersion;
    }
}

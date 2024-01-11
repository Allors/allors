namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;

public class Effect : IEffect, IUpstream
{
    private static readonly IDictionary<ISignal, long> EmptyDictionary = new Dictionary<ISignal, long>();

    private readonly Dispatcher dispatcher;
    private readonly Action<ITracker> dependencies;

    private IDictionary<ISignal, long> versionBySignal;
    private IDictionary<ISignal, long> trackingVersionBySignal;

    private bool shouldRaise;
    
    public Effect(Dispatcher dispatcher, Action<ITracker> dependencies, Action action)
    {
        this.dispatcher = dispatcher;
        this.dependencies = dependencies;
        this.Action = action;

        this.versionBySignal = EmptyDictionary;
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
        this.trackingVersionBySignal = new Dictionary<ISignal, long>();
        this.dependencies(this);

        if (this.shouldRaise)
        {
            this.shouldRaise = false;
            this.Action();
        }

        this.versionBySignal = this.trackingVersionBySignal;
        this.trackingVersionBySignal = null;
        
        this.dispatcher.UpdateTracked(this, this.versionBySignal.Keys);
    }

    public void Track(ISignal signal)
    {
        if (signal == null || this.trackingVersionBySignal.ContainsKey(signal))
        {
            return;
        }
        
        var trackingWorkspaceVersion = signal.Version;
        if (this.versionBySignal.TryGetValue(signal, out var version))
        {
            if (version != trackingWorkspaceVersion)
            {
                this.shouldRaise = true;
            }
        }
        else
        {
            this.shouldRaise = true;
        }

        this.trackingVersionBySignal[signal] = trackingWorkspaceVersion;
    }
}

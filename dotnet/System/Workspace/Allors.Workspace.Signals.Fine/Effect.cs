﻿namespace Allors.Workspace.Signals.Fine;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Effect : IEffect, IDependencyTracker
{
    private static readonly IDictionary<IOperand, long> EmptyDictionary = new Dictionary<IOperand, long>();

    private readonly Dispatcher dispatcher;

    private IDictionary<IOperand, long> workspaceVersionByOperand;
    private IDictionary<IOperand, long> trackingWorkspaceVersionByOperand;

    private bool shouldRaise;

    public Effect(Dispatcher dispatcher, Action<IDependencyTracker> dependencies, Action action)
    {
        this.dispatcher = dispatcher;
        this.Dependencies = dependencies;
        this.Action = action;

        this.workspaceVersionByOperand = EmptyDictionary;
    }

    public Action<IDependencyTracker> Dependencies { get; }

    public Action Action { get; }

    public void Dispose()
    {
        this.dispatcher.RemoveEffect(this);
    }

    public void Raise()
    {
        this.trackingWorkspaceVersionByOperand = new Dictionary<IOperand, long>();
        this.Dependencies(this);

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
}

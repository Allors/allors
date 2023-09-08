﻿namespace Allors.Workspace.Signals.Coarse;

public class ValueSignal<T> : IValueSignal<T>
{
    private readonly Dispatcher dispatcher;
    private long workspaceVersion;
    private T value;

    public ValueSignal(Dispatcher dispatcher, T value)
    {
        this.dispatcher = dispatcher;
        this.Value = value;
    }
    
    object ISignal.Value => this.Value;

    public T Value
    {
        get => this.value;
        set
        {
            if (!Equals(value, this.value))
            {
                this.value = value;
                ++this.workspaceVersion;
                this.dispatcher.ValueSignalOnChangedValue();
            }
        }
    }

    object IValueSignal.Value { get; set; }

    public long WorkspaceVersion => this.workspaceVersion;
}
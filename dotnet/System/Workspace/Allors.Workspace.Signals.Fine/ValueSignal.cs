namespace Allors.Workspace.Signals.Fine;

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
            }
        }
    }

    object IValueSignal.Value { get; set; }

    public long WorkspaceVersion => this.workspaceVersion;
}

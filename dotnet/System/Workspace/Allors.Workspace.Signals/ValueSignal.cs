namespace Allors.Workspace.Signals;

public class ValueSignal<T> : ISignal<T>
{
    private T value;
    
    private readonly InvalidationRequestedEventArgs invalidationRequestedEventArgs;

    public ValueSignal(T value)
    {
        this.Value = value;
        this.invalidationRequestedEventArgs = new InvalidationRequestedEventArgs(this);
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
                this.OnChanged();
            }
        }
    }

    public event InvalidationRequestedEventHandler InvalidationRequested;

    private void OnChanged()
    {
        var handlers = this.InvalidationRequested;
        handlers?.Invoke(this, this.invalidationRequestedEventArgs);
    }
}

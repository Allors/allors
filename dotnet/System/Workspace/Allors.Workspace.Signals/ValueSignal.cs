namespace Allors.Workspace.Signals;

public class ValueSignal<T> : ISignal<T>
{
    private T value;
    
    private readonly ChangedEventArgs changedEventArgs;

    public ValueSignal(T value)
    {
        this.Value = value;
        this.changedEventArgs = new ChangedEventArgs(this);
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

    public event ChangedEventHandler Changed;

    private void OnChanged()
    {
        var handlers = this.Changed;
        handlers?.Invoke(this, changedEventArgs);
    }
}

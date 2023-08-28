namespace Allors.Workspace.Signals.Default;

public class ValueSignal<T> : IValueSignal<T>, IProducer
{
    public ValueSignal(T value)
    {
        this.Value = value;
    }
    
    object ISignal.Value => this.Value;
    
    public T Value { get; set; }
    
    object IValueSignal.Value { get; set; }
}

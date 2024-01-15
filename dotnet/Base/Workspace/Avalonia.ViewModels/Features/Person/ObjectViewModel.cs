namespace Avalonia.ViewModels;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Signals;
using global::ReactiveUI;

public abstract class ObjectViewModel<T> : ViewModel, IDisposable
    where T : class, IObject
{
    protected readonly ValueSignal<T> model;

    protected IEffect modelChanged;

    protected ObjectViewModel(T model)
    {
        this.model = new ValueSignal<T>(model);

        this.modelChanged = new Effect(() => this.RaisePropertyChanged(nameof(this.Model)), this.model);
    }

    [Browsable(false)]
    public ValueSignal<T> Model => this.model;

    protected T ModelValue(ITracker tracker) => this.model.Track(tracker).Value;

    protected abstract void OnDispose();

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        this.modelChanged?.Dispose();
        this.OnDispose();
    }
}

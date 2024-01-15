namespace Avalonia.ViewModels;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Signals;
using global::ReactiveUI;

public abstract class ObjectViewModel<T> : ViewModel, IDisposable
    where T : class, IObject
{
    protected readonly IDispatcher dispatcher;
    protected readonly ValueSignal<T> model;

    protected IEffect modelChanged;

    protected ObjectViewModel(T model, IDispatcher? dispatcher = null)
    {
        var workspace = model.Strategy.Workspace;
        this.dispatcher = dispatcher ?? workspace.Services.Get<IDispatcherBuilder>().Build(workspace);

        this.model = this.dispatcher.CreateValueSignal(model);

        this.modelChanged = this.dispatcher.CreateEffect(tracker => this.model.Track(tracker), () => this.RaisePropertyChanged(nameof(this.Model)));
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

namespace Workspace.WinForms.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;

public abstract class ObjectViewModel<T> : ObservableObject, IDisposable
    where T : class, IObject
{
    protected readonly ValueSignal<T> model;

    protected Effect modelChanged;

    protected ObjectViewModel(T model)
    {
        this.model = new ValueSignal<T>(model);

        this.modelChanged = new Effect(() => this.OnPropertyChanged(nameof(this.Model)), this.model);
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

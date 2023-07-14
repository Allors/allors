namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;

public class RoleAdapter<T> : IDisposable
{
    public RoleAdapter(IPropertyChange viewModel, IUnitRole<T> role)
    {
        this.Role = role;
        this.ChangeNotification = new WeakReference<IPropertyChange>(viewModel);

        this.Role.PropertyChanged += this.Role_PropertyChanged;
    }

    public IUnitRole<T> Role { get; }

    public WeakReference<IPropertyChange> ChangeNotification { get; }

    public T Value
    {
        get => this.Role.Value;
        set => this.Role.Value = value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Role.PropertyChanged -= this.Role_PropertyChanged;
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }
}

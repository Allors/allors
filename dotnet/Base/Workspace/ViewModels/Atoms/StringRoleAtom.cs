namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;

public class StringRoleAtom : IDisposable
{
    public StringRoleAtom(IStringRole role, IPropertyChange propertyChange)
    {
        this.Role = role;
        this.ChangeNotification = new WeakReference<IPropertyChange>(propertyChange);

        this.Role.PropertyChanged += this.Role_PropertyChanged;
    }

    public IStringRole Role { get; private set; }

    public WeakReference<IPropertyChange> ChangeNotification { get; private set; }

    public String Value
    {
        get => this.Role.Value;
        set => this.Role.Value = value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Role.PropertyChanged -= this.Role_PropertyChanged;
    }

    private void Role_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }
}

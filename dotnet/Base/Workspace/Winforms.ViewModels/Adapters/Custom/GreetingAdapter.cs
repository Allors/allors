namespace Workspace.WinForms.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm.Adapters;

public class GreetingAdapter : IDisposable
{
    public GreetingAdapter(Person person, IViewModel viewModel, string propertyName = "Greeting")
    {
        this.Person = person;
        this.ChangeNotification = new WeakReference<IViewModel>(viewModel);
        this.PropertyName = propertyName;

        this.Roles = new IRole[] { this.Person.FirstName, this.Person.LastName, };

        foreach (var role in this.Roles)
        {
            role.PropertyChanged += this.Role_PropertyChanged;
        }

        this.Calculate();
    }

    public Person Person { get; }

    public IRole[] Roles { get; private set; }

    public WeakReference<IViewModel> ChangeNotification { get; private set; }

    public string PropertyName { get; }

    public string Value
    {
        get;
        private set;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var role in this.Roles)
        {
            role.PropertyChanged -= this.Role_PropertyChanged;
        }
    }

    private void Calculate()
    {
        this.Value = $"Hello {this.Person.FirstName.Value}";
    }

    private void Role_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        this.Calculate();
        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.PropertyName));
    }

}

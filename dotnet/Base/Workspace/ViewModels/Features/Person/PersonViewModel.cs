namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject, IDisposable
{
    private readonly Person person;

    private readonly Action unregisterEventHandlers;

    public PersonViewModel(Person person)
    {
        this.person = person;

        PropertyChangedEventHandler firstNameOnPropertyChanged = (_, _) => this.OnPropertyChanged(nameof(this.FirstName));

        this.person.FirstName.PropertyChanged += firstNameOnPropertyChanged;

        this.unregisterEventHandlers = () =>
        {
            this.person.FirstName.PropertyChanged -= firstNameOnPropertyChanged;
        };
    }

    public string FirstName
    {
        get => this.person.FirstName.Value;
        set => this.person.FirstName.Value = value;
    }

    public void Dispose()
    {
        this.unregisterEventHandlers();
    }
}

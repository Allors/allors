namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject, IPropertyChange
{
    private readonly StringRoleAtom firstName;

    public PersonViewModel(Person person)
    {
        this.firstName = new StringRoleAtom(person.FirstName, this);
    }

    public string FirstName
    {
        get => this.firstName.Value;
        set => this.firstName.Value = value;
    }

    void IPropertyChange.OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.OnPropertyChanged(e);
    }
}

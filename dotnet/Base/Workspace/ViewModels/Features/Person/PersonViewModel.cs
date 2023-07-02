namespace Workspace.ViewModels.Features;

using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject
{
    private readonly Person person;

    public PersonViewModel(Person person) => this.person = person;

    public string FirstName
    {
        get => this.person.FirstName.Value;
        set => this.SetProperty(this.person.FirstName.Value, value, this.person, (v, w) => v.FirstName.Value = w);
    }

    public string LastName
    {
        get => this.person.LastName.Value;
        set => this.SetProperty(this.person.LastName.Value, value, this.person, (v, w) => v.LastName.Value = w);
    }
}

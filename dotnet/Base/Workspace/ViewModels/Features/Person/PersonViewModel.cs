namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject, IPropertyChange
{
    private readonly StringRoleAtom firstName;
    private readonly ExpressionAtom<string> fullName;
    private readonly GreetingAtom greeting;

    public PersonViewModel(Person person)
    {
        this.firstName = new StringRoleAtom(person.FirstName, this);
        this.fullName = new ExpressionAtom<string>(
            new IRole[]
            {
                person.FirstName,
                person.LastName
            },
            () => person.FirstName.Value + " " + person.LastName,
            this,
            "FullName");
        this.greeting = new GreetingAtom(person, this);
    }

    public string FirstName
    {
        get => this.firstName.Value;
        set => this.firstName.Value = value;
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;

    #region OnPropertyChanged
    void IPropertyChange.OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.OnPropertyChanged(e);
    }
    #endregion
}

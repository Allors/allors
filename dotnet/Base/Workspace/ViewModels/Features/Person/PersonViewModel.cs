namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
using Allors.Workspace.Domain;
using Controllers;

public partial class PersonViewModel : ViewModel
{
    private readonly RoleAdapter<string> firstName;
    private readonly ExpressionAdapter<string> fullName;
    private readonly GreetingAdapter greeting;

    public PersonViewModel(Person person)
    {
        this.firstName = new RoleAdapter<string>(this, person.FirstName);
        this.fullName = new ExpressionAdapter<string>(this,
            new IRole[]
            {
                person.FirstName,
                person.LastName
            }, () => person.FirstName.Value + " " + person.LastName, "FullName");
        this.greeting = new GreetingAdapter(person, this);
    }

    public string FirstName
    {
        get => this.firstName.Value;
        set => this.firstName.Value = value;
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;
}

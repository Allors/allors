namespace Workspace.WinForms.ViewModels.Features;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm;
using Allors.Workspace.Mvvm.Adapters;

public partial class PersonViewModel : ViewModel<Person>
{
    private readonly RoleAdapter<string> firstName;
    private readonly PathAdapter<string> poBox;
    private readonly ExpressionAdapter<string> fullName;
    private readonly GreetingAdapter greeting;

    public PersonViewModel(Person model)
    {
        this.Model = model;
        this.firstName = new RoleAdapter<string>(this, model.FirstName);
        this.fullName = new ExpressionAdapter<string>(this,
            new IRole[]
            {
                model.FirstName,
                model.LastName
            }, () => model.FirstName.Value + " " + model.LastName, "FullName");
        this.greeting = new GreetingAdapter(model, this);
    }

    public override Person Model { get; }

    public string FirstName
    {
        get => this.firstName.Value;
        set => this.firstName.Value = value;
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;
}

namespace Workspace.Blazor.ViewModels.Features.Person.Overview;
using Allors.Workspace;
using Allors.Workspace.Mvvm;
using Allors.Workspace.Mvvm.Adapters;
using ViewModels.Features;
using Person = Allors.Workspace.Domain.Person;

public partial class PersonViewModel : ViewModel<Person>
{
    private readonly UnitRoleAdapter<string> firstName;
    private readonly PathAdapter<string> poBox;
    private readonly ExpressionAdapter<string> fullName;
    private readonly GreetingAdapter greeting;

    public PersonViewModel(Person model)
    {
        this.Model = model;
        this.firstName = new UnitRoleAdapter<string>(this, model.FirstName);
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

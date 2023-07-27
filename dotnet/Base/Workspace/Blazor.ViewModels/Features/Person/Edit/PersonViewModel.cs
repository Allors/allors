namespace Workspace.Blazor.ViewModels.Features.Person.Edit;
using Allors.Workspace;
using ViewModels.Controllers;
using ViewModels.Features;
using Person = Allors.Workspace.Domain.Person;

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
        //this.poBox = model.Meta.PathAdapter(this, v => v.MailboxAddress.ObjectType.PoBox);
    }

    public override Person Model { get; }

    public string FirstName
    {
        get => this.firstName.Value;
        set => this.firstName.Value = value;
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;

    public string PoBox
    {
        get => this.poBox.Value;
        set => this.poBox.Value = value;
    }
}

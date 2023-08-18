namespace Workspace.Blazor.ViewModels.Features.Person.Edit;

using System.Linq.Expressions;
using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Meta;
using Allors.Workspace.Mvvm;
using Allors.Workspace.Mvvm.Adapters;
using Allors.Workspace.Mvvm.Generator;
using ViewModels.Features;
using Person = Allors.Workspace.Domain.Person;

public partial class PersonViewModel : ViewModel<Person>
{
    [AdapterProperty] private readonly UnitRoleAdapter<string> firstName;
    [AdapterProperty] private readonly UnitRoleExpressionAdapter<Person, string> poBox;
    private readonly ExpressionAdapter<Person, string> fullName;
    private readonly GreetingAdapter greeting;

    public PersonViewModel(Person model)
    {
        this.Model = model;

        var workspace = model.Strategy.Workspace;
        var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
        var reactiveExpressionBuilder = workspace.Services.Get<IReactiveExpressionBuilder>();

        this.firstName = new UnitRoleAdapter<string>(this, model.FirstName);
        this.fullName = new ExpressionAdapter<Person, string>(this, reactiveExpressionBuilder.Build(model, reactiveFuncBuilder.Build((Expression<Func<Person, string>>)(v => v.FirstName.Value + " " + v.LastName.Value))), "FullName");
        this.greeting = new GreetingAdapter(model, this);
        this.poBox = new UnitRoleExpressionAdapter<Person, string>(this, reactiveExpressionBuilder.Build(model, reactiveFuncBuilder.Build((Expression<Func<Person, IUnitRole<String>>>)(v => v.MailboxAddress.Value.PoBox))), "PoBox");

        var temp = this.FirstName;
    }

    public override Person Model { get; }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;
}

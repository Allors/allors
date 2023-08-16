namespace Workspace.WinForms.ViewModels.Features;

using System.Linq.Expressions;
using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm;
using Allors.Workspace.Mvvm.Adapters;

public partial class PersonViewModel : ViewModel<Person>
{
    private readonly UnitRoleAdapter<string> firstName;
    private readonly ExpressionAdapter<Person, string> poBox;
    private readonly ExpressionAdapter<Person, string> fullName;
    private readonly GreetingAdapter greeting;

    public PersonViewModel(Person model)
    {
        var workspace = model.Strategy.Workspace;
        var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
        var reactiveExpressionBuilder = workspace.Services.Get<IReactiveExpressionBuilder>();

        this.Model = model;
        this.firstName = new UnitRoleAdapter<string>(this, model.FirstName);

        Expression<Func<Person, string>> expression = (v => v.FirstName.Value + " " + v.LastName.Value);
        this.fullName = new(this, reactiveExpressionBuilder.Build(model, reactiveFuncBuilder.Build(expression)), "FullName");

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

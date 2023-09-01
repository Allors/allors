namespace Workspace.WinForms.ViewModels.Features;

using System.Linq.Expressions;
using Allors.Workspace;
using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject
{
    private readonly IValueSignal<Person> model;

    private readonly IComputedSignal<IUnitRole<string>> firstName;
    private readonly IComputedSignal<string> fullName;
    private readonly IComputedSignal<string> greeting;

    private readonly IComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    private readonly IComputedSignal<IUnitRole<string>?> poBox;

    public PersonViewModel(Person model)
    {
        var workspace = model.Strategy.Workspace;
        var dispatcher = workspace.Services.Get<IDispatcherBuilder>().Build(workspace);

        this.model = dispatcher.CreateValueSignal(model);

        this.firstName = dispatcher.CreateComputedSignal(tracker => this.model.TrackedValue(tracker).FirstName.Track(tracker));
        this.fullName = dispatcher.CreateComputedSignal(tracker =>
        {
            var personValue = this.model.TrackedValue(tracker);
            string firstNameValue = personValue.FirstName.TrackedValue(tracker);
            string lastNameValue = personValue.LastName.TrackedValue(tracker);
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = dispatcher.CreateComputedSignal(tracker =>
        {
            var fullNameValue = this.fullName.TrackedValue(tracker);
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = dispatcher.CreateComputedSignal(tracker => this.model.TrackedValue(tracker).MailboxAddress.Track(tracker));
        this.poBox = dispatcher.CreateComputedSignal(tracker => this.mailboxAddress.TrackedValue(tracker)?.TrackedValue(tracker)?.PoBox.Track(tracker));
    }

    public Person Model { get => this.model.Value; }

    public string FirstName
    {
        get => this.firstName.Value.Value;
        set => this.firstName.Value.Value = value;
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;
}

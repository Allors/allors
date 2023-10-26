namespace Avalonia.ViewModels;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Signals;

public partial class PersonGeneratorViewModel : ObjectViewModel<Person>
{
    private readonly IValueSignal<Person> model;

    [SignalProperty] private readonly IComputedSignal<IUnitRole<string>?> firstName;
    [SignalProperty] private readonly IComputedSignal<string?> fullName;
    [SignalProperty] private readonly IComputedSignal<string?> greeting;

    private readonly IComputedSignal<ICompositeRole<MailboxAddress>?> mailboxAddress;
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string>?> poBox;

    public PersonGeneratorViewModel(Person model) : base(model)
    {
        this.firstName = dispatcher.CreateComputedSignal(tracker => this.ModelValue(tracker).FirstName(tracker));
        this.fullName = dispatcher.CreateComputedSignal(tracker =>
        {
            var person = this.ModelValue(tracker);
            string? firstNameValue = person.FirstName(tracker)?.Value;
            string? lastNameValue = person.LastName(tracker)?.Value;
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = dispatcher.CreateComputedSignal(tracker =>
        {
            var fullNameValue = this.fullName.Track(tracker).Value;
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = dispatcher.CreateComputedSignal(tracker => this.ModelValue(tracker)?.MailboxAddress(tracker));
        this.poBox = this.dispatcher.CreateComputedSignal(tracker => this.mailboxAddress.Value?.Track(tracker)?.Value?.PoBox(tracker));

        this.OnInitEffects(dispatcher);
    }

    protected override void OnDispose() => this.OnDisposeEffects();
}

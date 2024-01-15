namespace Workspace.WinForms.ViewModels.Features;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Signals;

public partial class PersonGeneratorViewModel : ObjectViewModel<Person>
{
    private readonly ValueSignal<Person> model;

    [SignalProperty] private readonly ComputedSignal<IUnitRole<string>?> firstName;
    [SignalProperty] private readonly ComputedSignal<string?> fullName;
    [SignalProperty] private readonly ComputedSignal<string?> greeting;

    private readonly ComputedSignal<ICompositeRole<MailboxAddress>?> mailboxAddress;
    [SignalProperty] private readonly ComputedSignal<IUnitRole<string>?> poBox;

    public PersonGeneratorViewModel(Person model) : base(model)
    {
        this.firstName = new ComputedSignal<IUnitRole<string>?>(tracker => this.ModelValue(tracker).FirstName(tracker));
        this.fullName = new ComputedSignal<string?>(tracker =>
        {
            var person = this.ModelValue(tracker);
            string? firstNameValue = person.FirstName(tracker)?.Value;
            string? lastNameValue = person.LastName(tracker)?.Value;
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = new ComputedSignal<string?>(tracker =>
        {
            var fullNameValue = this.fullName.Track(tracker).Value;
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = new ComputedSignal<ICompositeRole<MailboxAddress>?>(tracker => this.ModelValue(tracker)?.MailboxAddress(tracker));
        this.poBox = new ComputedSignal<IUnitRole<string>?>(tracker => this.mailboxAddress.Value?.Track(tracker)?.Value?.PoBox(tracker));

        this.OnInitEffects();
    }

    protected override void OnDispose() => this.OnDisposeEffects();
}

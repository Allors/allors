namespace Avalonia.ViewModels;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Signals;
using global::ReactiveUI;

public partial class PersonManualViewModel : ReactiveObject, IDisposable
{
    private readonly ValueSignal<Person> model;

    private readonly ComputedSignal<IUnitRole<string>> firstName;
    private readonly ComputedSignal<string?> fullName;
    private readonly ComputedSignal<string?> greeting;

    private readonly ComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    private readonly ComputedSignal<IUnitRole<string?>?> poBox;

    private readonly IEffect firstNameChanged;
    private readonly IEffect fullNameChanged;
    private readonly IEffect greetingChanged;
    private readonly IEffect poBoxChanged;

    public PersonManualViewModel(Person model)
    {
        var workspace = model.Strategy.Workspace;

        this.model = new ValueSignal<Person>(model);

        this.firstName = new ComputedSignal<IUnitRole<string>>(tracker => this.model.Track(tracker).Value.FirstName.Track(tracker));
        this.fullName = new ComputedSignal<string?>(tracker =>
        {
            var personValue = this.model.Track(tracker).Value;
            string firstNameValue = personValue.FirstName.Track(tracker).Value;
            string lastNameValue = personValue.LastName.Track(tracker).Value;
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = new ComputedSignal<string?>(tracker =>
        {
            var fullNameValue = this.fullName.Track(tracker).Value;
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = new ComputedSignal<ICompositeRole<MailboxAddress>>(tracker => this.model.Track(tracker).Value.MailboxAddress.Track(tracker));
        this.poBox = new ComputedSignal<IUnitRole<string>>(tracker => this.mailboxAddress.Track(tracker).Value?.Track(tracker).Value?.PoBox.Track(tracker));

        this.firstNameChanged = new Effect(() => this.RaisePropertyChanged(nameof(FirstName)), this.firstName);
        this.fullNameChanged = new Effect(() => this.RaisePropertyChanged(nameof(FullName)), this.fullName);
        this.greetingChanged = new Effect(() => this.RaisePropertyChanged(nameof(Greeting)), this.greeting);
        this.poBoxChanged = new Effect(() => this.RaisePropertyChanged(nameof(Greeting)), this.poBox);
    }

    public Person Model { get => this.model.Value; }

    public string FirstName
    {
        get => this.firstName.Value.Value;
        set => this.firstName.Value.Value = value;
    }

    public string? PoBox
    {
        get => this.poBox.Value?.Value;
        set
        {
            if (this.poBox.Value != null)
            {
                this.poBox.Value.Value = value;
            }
        }
    }

    public string FullName => this.fullName.Value;

    public string Greeting => this.greeting.Value;

    public void Dispose()
    {
        this.firstNameChanged.Dispose();
        this.fullNameChanged.Dispose();
        this.greetingChanged.Dispose();
        this.poBoxChanged.Dispose();
    }
}

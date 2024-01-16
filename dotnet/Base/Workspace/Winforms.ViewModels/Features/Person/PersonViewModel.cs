namespace Workspace.WinForms.ViewModels.Features;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Meta;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject, IDisposable
{
    private readonly ValueSignal<Person> model;

    private readonly ComputedSignal<IUnitRole<string>> firstName;
    private readonly ComputedSignal<string?> fullName;
    private readonly ComputedSignal<string?> greeting;

    private readonly ComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    private readonly ComputedSignal<IUnitRole<string?>?> poBox;

    private readonly Effect firstNameChanged;
    private readonly Effect fullNameChanged;
    private readonly Effect greetingChanged;
    private readonly Effect poBoxChanged;

    public PersonViewModel(Person model)
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
        this.poBox = new ComputedSignal<IUnitRole<String>>(tracker => this.mailboxAddress.Track(tracker).Value?.Track(tracker).Value?.PoBox.Track(tracker));

        this.firstNameChanged = new Effect(() => this.OnPropertyChanged(nameof(FirstName)), this.firstName);
        this.fullNameChanged = new Effect(() => this.OnPropertyChanged(nameof(FullName)), this.fullName);
        this.greetingChanged = new Effect(() => this.OnPropertyChanged(nameof(Greeting)), this.greeting);
        this.poBoxChanged = new Effect(() => this.OnPropertyChanged(nameof(Greeting)), this.poBox);
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

namespace Avalonia.ViewModels;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Signals;
using global::ReactiveUI;

public partial class PersonViewModel : ReactiveObject, IDisposable
{
    private readonly ComputedSignal<IUnitRole<string>> firstName;
    private readonly ComputedSignal<string?> fullName;
    private readonly ComputedSignal<string?> greeting;

    private readonly ComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    private readonly ComputedSignal<IUnitRole<string>?> poBox;

    private readonly NamedEffect propertyChangedEffect;

    public PersonViewModel(Person model)
    {
        this.Model = model;

        this.firstName = new ComputedSignal<IUnitRole<string>>(tracker => this.Model.FirstName.Track(tracker));
        this.fullName = new ComputedSignal<string?>(tracker =>
        {
            string firstNameValue = this.Model.FirstName.Track(tracker).Value;
            string lastNameValue = this.Model.LastName.Track(tracker).Value;
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = new ComputedSignal<string?>(tracker =>
        {
            var fullNameValue = this.fullName.Track(tracker).Value;
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = new ComputedSignal<ICompositeRole<MailboxAddress>>(tracker => this.Model.MailboxAddress.Track(tracker));
        this.poBox = new ComputedSignal<IUnitRole<string>?>(tracker => this.mailboxAddress.Value?.Track(tracker).Value?.PoBox.Track(tracker));

        this.propertyChangedEffect = new NamedEffect(this.RaisePropertyChanged, v =>
        {
            v.Add(this.firstName);
            v.Add(this.fullName, nameof(FullName));
            v.Add(this.greeting, nameof(Greeting));
            v.Add(this.poBox);
        });
    }

    public Person Model { get; }

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
        this.propertyChangedEffect?.Dispose();

        this.mailboxAddress?.Dispose();
        this.poBox?.Dispose();
    }
}

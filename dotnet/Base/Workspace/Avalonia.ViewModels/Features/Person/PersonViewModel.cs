namespace Avalonia.ViewModels;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Signals;
using global::ReactiveUI;

public partial class PersonViewModel : ReactiveObject, IDisposable
{
    private readonly ComputedSignal<IUnitRole<string>> firstName;
    private readonly IUnitRole<string> lastName;
    private readonly ComputedSignal<IUnitRole<decimal?>> weight;
    private readonly ComputedSignal<string?> fullName;
    private readonly ComputedSignal<string?> greeting;
    private readonly ComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    private readonly ComputedSignal<IUnitRole<string>?> poBox;

    private readonly IEffect propertyChangedEffect;

    public PersonViewModel(Person model)
    {
        this.Model = model;

        this.firstName = new ComputedSignal<IUnitRole<string>>(tracker => this.Model.FirstName.Track(tracker));
        this.lastName = this.Model.LastName;
        this.weight = new ComputedSignal<IUnitRole<decimal?>>(tracker => this.Model.Weight.Track(tracker));
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
            v.Add(this.firstName, nameof(FirstName));
            v.Add(this.lastName);
            v.Add(this.fullName, nameof(FullName));
            v.Add(this.greeting, nameof(Greeting));
            v.Add(this.poBox, nameof(PoBox));
        });
    }

    public Person Model { get; }

    public string FirstName
    {
        get => this.firstName.Value.Value;
        set => this.firstName.Set(value);
    }

    public string LastName
    {
        get => this.lastName.Value;
        set => this.lastName.Value = value;
    }

    public decimal? Weight
    {
        get => this.weight.Value.Value;
        set => this.weight.Set(value);
    }

    public string PoBox
    {
        get => this.poBox.Value?.Value ?? string.Empty;
        set => this.firstName.Set(value);
    }

    public string FullName => this.fullName.Value ?? string.Empty;

    public string Greeting => this.greeting.Value ?? string.Empty;

    public void Dispose()
    {
        this.propertyChangedEffect?.Dispose();
    }
}

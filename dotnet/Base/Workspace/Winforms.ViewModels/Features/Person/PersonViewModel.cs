namespace Workspace.WinForms.ViewModels.Features;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Meta;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonViewModel : ObservableObject, IDisposable
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

        this.propertyChangedEffect = new NamedEffect(this.OnPropertyChanged, v =>
        {
            v.Add(this.firstName);
            v.Add(this.lastName);
            v.Add(this.fullName, nameof(FullName));
            v.Add(this.greeting, nameof(Greeting));
            v.Add(this.poBox);
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

        this.mailboxAddress?.Dispose();
        this.poBox?.Dispose();
    }
}

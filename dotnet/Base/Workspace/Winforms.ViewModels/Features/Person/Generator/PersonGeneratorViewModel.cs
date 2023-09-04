namespace Workspace.WinForms.ViewModels.Features;

using Allors.Workspace;
using Allors.Workspace.Domain;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class PersonGeneratorViewModel : ObservableObject, IDisposable
{
    private readonly IValueSignal<Person> model;

    [SignalProperty] private readonly IComputedSignal<IUnitRole<string>> firstName;
    [SignalProperty] private readonly IComputedSignal<string?> fullName;
    [SignalProperty] private readonly IComputedSignal<string?> greeting;

    private readonly IComputedSignal<ICompositeRole<MailboxAddress>> mailboxAddress;
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string?>> poBox;

    public PersonGeneratorViewModel(Person model)
    {
        var workspace = model.Strategy.Workspace;
        var dispatcher = workspace.Services.Get<IDispatcherBuilder>().Build(workspace);

        this.model = dispatcher.CreateValueSignal(model);

        this.firstName = dispatcher.CreateComputedSignal(tracker => this.model.Track(tracker).Value.FirstName.Track(tracker));
        this.fullName = dispatcher.CreateComputedSignal(tracker =>
        {
            var personValue = this.model.Track(tracker).Value;
            string firstNameValue = personValue.FirstName.Track(tracker).Value;
            string lastNameValue = personValue.LastName.Track(tracker).Value;
            return $"{firstNameValue} {lastNameValue}".Trim();
        });
        this.greeting = dispatcher.CreateComputedSignal(tracker =>
        {
            var fullNameValue = this.fullName.Track(tracker).Value;
            return $"Hello {fullNameValue}!";
        });

        this.mailboxAddress = dispatcher.CreateComputedSignal(tracker => this.model.Track(tracker).Value.MailboxAddress.Track(tracker));
        this.poBox = dispatcher.CreateComputedSignal(tracker => this.mailboxAddress.Track(tracker).Value?.Track(tracker).Value?.PoBox.Track(tracker));

        this.OnInitEffects(dispatcher);
    }

    public Person Model { get => this.model.Value; }
    
    public void Dispose()
    {
        this.OnDisposeEffects();
    }
}

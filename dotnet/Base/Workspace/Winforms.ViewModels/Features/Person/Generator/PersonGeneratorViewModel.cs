﻿namespace Workspace.WinForms.ViewModels.Features;

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

    private readonly IEffect firstNameChanged;
    private readonly IEffect fullNameChanged;
    private readonly IEffect greetingChanged;
    private readonly IEffect poBoxChanged;

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

        this.firstNameChanged = dispatcher.CreateEffect(tracker => this.firstName.Track(tracker).Value?.Track(tracker), () => this.OnPropertyChanged(nameof(FirstName)));
        this.fullNameChanged = dispatcher.CreateEffect(tracker => this.fullName.Track(tracker), () => this.OnPropertyChanged(nameof(FullName)));
        this.greetingChanged = dispatcher.CreateEffect(tracker => this.greeting.Track(tracker), () => this.OnPropertyChanged(nameof(Greeting)));
        this.poBoxChanged = dispatcher.CreateEffect(tracker => this.poBox.Track(tracker).Value?.Track(tracker), () => this.OnPropertyChanged(nameof(Greeting)));
    }

    public Person Model { get => this.model.Value; }
    
    public void Dispose()
    {
        this.firstNameChanged.Dispose();
        this.fullNameChanged.Dispose();
        this.greetingChanged.Dispose();
        this.poBoxChanged.Dispose();
    }
}

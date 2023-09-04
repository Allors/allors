namespace Workspace.WinForms.ViewModels.Features;

using System.Collections.ObjectModel;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Meta;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Person = Allors.Workspace.Domain.Person;
using Task = Task;

public partial class PersonManualFormViewModel : ObservableObject, IDisposable
{
    private readonly IValueSignal<PersonManualViewModel?> selected;

    private readonly IEffect selectedChanged;

    public PersonManualFormViewModel(IWorkspace workspace, IMessageService messageService)
    {
        this.Workspace = workspace;
        this.MessageService = messageService;
        var dispatcher = workspace.Services.Get<IDispatcherBuilder>().Build(workspace);

        this.selected = dispatcher.CreateValueSignal<PersonManualViewModel>(null);

        this.selectedChanged = dispatcher.CreateEffect(tracker => this.selected.Track(tracker), () => this.OnPropertyChanged(nameof(Selected)));
    }

    public IWorkspace Workspace { get; set; }

    public IMessageService MessageService { get; }

    public ObservableCollection<PersonManualViewModel> People { get; } = new();

    public PersonManualViewModel? Selected
    {
        get => this.selected.Value;
        set
        {
            this.selected.Value = value;
        }
    }

    [RelayCommand]
    private void ShowDialog()
    {
        var result = this.MessageService.ShowDialog("Yes or No?", "The Question");
        Console.WriteLine(result);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        var m = this.Workspace.Services.Get<M>();

        var pull = new Pull
        {
            Extent = new Filter(m.Person),
            Results = new[]
            {
                new Result
                {
                    Include = m.Person.Nodes(v=>v.MailboxAddress.Node())
                }
            }
        };

        var result = await this.Workspace.PullAsync(pull);
        var people = result.GetCollection<Person>();

        this.People.Clear();
        foreach (var person in people)
        {
            this.People.Add(new PersonManualViewModel(person));
        }

        this.OnPropertyChanged(nameof(People));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var result = await this.Workspace.PushAsync();

        if (result.HasErrors)
        {
            this.MessageService.Show(result.ErrorMessage, "Error");
            return;
        }

        this.Workspace.Reset();

        await this.LoadAsync();
    }

    public void Dispose()
    {
        this.selectedChanged.Dispose();
    }
}

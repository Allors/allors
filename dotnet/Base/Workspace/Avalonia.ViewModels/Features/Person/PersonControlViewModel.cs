namespace Avalonia.ViewModels;

using System.Collections.ObjectModel;
using System.Reactive;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Domain;
using Allors.Workspace.Meta;
using Allors.Workspace.Signals;
using global::ReactiveUI;
using Task = System.Threading.Tasks.Task;

public class PersonControlViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly ValueSignal<PersonViewModel?> selected;

    private readonly Effect selectedChanged;

    public PersonControlViewModel(IWorkspace workspace, IMessageService messageService, IScreen screen)
    {
        this.Workspace = workspace;
        this.MessageService = messageService;
        this.HostScreen = screen;

        this.selected = new ValueSignal<PersonViewModel>(null);

        this.selectedChanged = new Effect(() =>
        {
            this.RaisePropertyChanged(nameof(this.Selected));
            this.RaisePropertyChanged(nameof(this.HasSelected));
        }, this.selected);

        this.Load = ReactiveCommand.CreateFromTask(this.SaveAsync);
        this.Save = ReactiveCommand.CreateFromTask(this.LoadAsync);
    }

    public IWorkspace Workspace { get; }

    public IMessageService MessageService { get; }

    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = "PersonManual";

    public ReactiveCommand<Unit, Unit> Load { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public bool PeopleHasRows => this.People.Count > 0;

    public ObservableCollection<PersonViewModel> People { get; } = new();

    public bool HasSelected => this.Selected != null;

    public PersonViewModel? Selected
    {
        get => this.selected.Value;
        set
        {
            this.selected.Value = value;
        }
    }

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
            this.People.Add(new PersonViewModel(person));
        }

        this.RaisePropertyChanged(nameof(People));
        this.RaisePropertyChanged(nameof(PeopleHasRows));
    }

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

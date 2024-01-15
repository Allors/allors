namespace Avalonia.ViewModels;

using System.Collections.ObjectModel;
using System.Reactive;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Meta;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Signals;
using global::ReactiveUI;
using Person = Allors.Workspace.Domain.Person;
using Task = Task;

public partial class PersonGeneratorControlViewModel : ViewModel, IRoutableViewModel, IDisposable
{
    [SignalProperty] private readonly ValueSignal<PersonGeneratorViewModel?> selected;

    private IEffect hasSelectedChanged;

    public PersonGeneratorControlViewModel(IWorkspace workspace, IMessageService messageService, IScreen screen)
    {
        this.Workspace = workspace;
        this.MessageService = messageService;
        this.HostScreen = screen;

        this.selected = new ValueSignal<PersonGeneratorViewModel>(null);

        this.Load = ReactiveCommand.CreateFromTask(this.SaveAsync);
        this.Save = ReactiveCommand.CreateFromTask(this.LoadAsync);

        this.hasSelectedChanged = new Effect(() => this.OnEffect(nameof(HasSelected)), this.selected);

        this.OnInitEffects();
    }

    public IWorkspace Workspace { get; set; }

    public IMessageService MessageService { get; }

    public string UrlPathSegment { get; } = "PersonGenerator";

    public IScreen HostScreen { get; }

    public ReactiveCommand<Unit, Unit> Load { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public bool PeopleHasRows => this.People.Count > 0;

    public ObservableCollection<PersonGeneratorViewModel> People { get; } = new();

    public bool HasSelected => this.Selected != null;

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
            this.People.Add(new PersonGeneratorViewModel(person));
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
        this.OnDisposeEffects();

        this.hasSelectedChanged?.Dispose();
    }
}

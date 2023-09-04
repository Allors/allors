namespace Workspace.WinForms.ViewModels.Features;

using System.Collections.ObjectModel;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Meta;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Signals;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Person = Allors.Workspace.Domain.Person;
using Task = Task;

public partial class PersonGeneratorFormViewModel : ObservableObject, IDisposable
{
    [SignalProperty] private readonly IValueSignal<PersonGeneratorViewModel?> selected;

    public PersonGeneratorFormViewModel(IWorkspace workspace, IMessageService messageService)
    {
        this.Workspace = workspace;
        this.MessageService = messageService;
        var dispatcher = workspace.Services.Get<IDispatcherBuilder>().Build(workspace);

        this.selected = dispatcher.CreateValueSignal<PersonGeneratorViewModel>(null);

        this.OnInitEffects(dispatcher);
    }

    public IWorkspace Workspace { get; set; }

    public IMessageService MessageService { get; }

    public ObservableCollection<PersonGeneratorViewModel> People { get; } = new();

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
            this.People.Add(new PersonGeneratorViewModel(person));
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
        this.OnDisposeEffects();
    }
}

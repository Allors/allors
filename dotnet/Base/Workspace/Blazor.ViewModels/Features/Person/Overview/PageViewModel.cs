﻿namespace Workspace.Blazor.ViewModels.Features.Person.Overview;

using Allors.Workspace;
using Allors.Workspace.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Allors.Workspace.Domain;
using Task = Task;
using Allors.Workspace.Meta;
using ViewModels.Services;

public partial class PageViewModel : ObservableObject, INavigateable
{
    private PersonViewModel selected;

    public PageViewModel(IWorkspace workspace, IMessageService messageService)
    {
        this.Workspace = workspace;
        this.MessageService = messageService;
    }
    public IWorkspace Workspace { get; set; }

    public IMessageService MessageService { get; }

    public long Id { get; private set; }

    public PersonViewModel Selected
    {
        get => this.selected;
        set
        {
            this.SetProperty(ref this.selected, value);
        }
    }

    [RelayCommand]
    private async Task LoadAsync(long objectId)
    {
        this.Id = objectId;

        var m = this.Workspace.Services.Get<M>();

        var pulls = new Pull[]
        {
            new Pull { Extent = new Filter(m.Country) },
            new()
            {
                ObjectId = objectId,
                Results = new[]
                {
                    new Result
                    {
                        Select = new Select
                        {
                            Include = m.MailboxAddress.Nodes()
                        }
                    }
                }
            },
        };

        var result = await this.Workspace.PullAsync(pulls);

        this.Selected = new PersonViewModel(result.GetObject<Person>());

        //var countries = new ObservableCollection<Country>(result.GetCollection<Country>());
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

        await this.LoadAsync(this.Id);
    }
}

namespace Workspace.WinForms.ViewModels.Features;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;

public partial class MainFormViewModel : ObservableObject
{
    public MainFormViewModel(IMdiService mdiService) => this.MdiService = mdiService;

    public IMdiService MdiService { get; }

    [RelayCommand]
    private void ShowPersonManual() => this.MdiService.Open(typeof(PersonManualFormViewModel));

    [RelayCommand]
    private void ShowPersonGenerator() => this.MdiService.Open(typeof(PersonGeneratorFormViewModel));
}

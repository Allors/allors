namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using System.Reflection.Emit;
using Allors.Workspace.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Binding.Builders;

public partial class PersonViewModel : ObservableObject, IDisposable
{
    private readonly Person person;

    private void personFirstName_PropertyChanged(object? sender, PropertyChangedEventArgs e) => this.OnPropertyChanged(nameof(this.FirstName));

    public PersonViewModel(Person person)
    {
        this.person = person;
        this.person.FirstName.PropertyChanged += this.personFirstName_PropertyChanged;
    }

    public string FirstName
    {
        get => this.person.FirstName.Value;
        set => this.person.FirstName.Value = value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.person.FirstName.PropertyChanged += this.personFirstName_PropertyChanged;
    }
}

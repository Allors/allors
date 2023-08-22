namespace Allors.Workspace.Signals.Tests;

using System.ComponentModel;
using Domain;
using Mvvm;

public class C1ViewModel : IObjectViewModel<C1>
{
    public C1ViewModel(C1 model)
    {
        this.Model = model;
    }

    public C1 Model { get; }

    public IList<PropertyChangedEventArgs> Events { get; } = new List<PropertyChangedEventArgs>();

    public void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.Events.Add(e);
    }
}
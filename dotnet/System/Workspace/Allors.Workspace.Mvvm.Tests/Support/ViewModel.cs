namespace Allors.Workspace.Signals.Tests
{
    using System.ComponentModel;
    using Mvvm.Adapters;

    public class ViewModel : IViewModel
    {
        public IList<PropertyChangedEventArgs> Events { get; } = new List<PropertyChangedEventArgs>();

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.Events.Add(e);
        }
    }
}

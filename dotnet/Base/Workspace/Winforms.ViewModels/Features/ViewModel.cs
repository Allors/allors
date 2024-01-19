namespace Allors.Workspace.Signals;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class ViewModel : INotifyPropertyChanged
{
    private PropertyChangedEventHandler? propertyChanged;

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add
        {
            this.propertyChanged += value;
            if (this.propertyChanged?.GetInvocationList().Length == 1)
            {
                this.OnPropertyChangedStarted();
            }
        }
        remove
        {
            this.propertyChanged -= value;
            if (this.propertyChanged?.GetInvocationList().Length == 0)
            {
                this.OnPropertyChangedStopped();
            }
        }
    }

    protected abstract void OnPropertyChangedStarted();

    protected abstract void OnPropertyChangedStopped();
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

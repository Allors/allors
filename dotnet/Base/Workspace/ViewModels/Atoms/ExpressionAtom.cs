namespace Workspace.ViewModels.Features;

using System.ComponentModel;
using Allors.Workspace;
public class ExpressionAtom<T> : IDisposable
{
    public ExpressionAtom(IRole[] roles, Func<T> expression, IPropertyChange propertyChange, string propertyName)
    {
        this.Roles = roles;
        this.Expression = expression;
        this.PropertyName = propertyName;
        this.ChangeNotification = new WeakReference<IPropertyChange>(propertyChange);

        foreach (var role in this.Roles)
        {
            role.PropertyChanged += this.Role_PropertyChanged;
        }

        this.Calculate();
    }

    public IRole[] Roles { get; private set; }

    public Func<T> Expression { get; }

    public string PropertyName { get; }

    public WeakReference<IPropertyChange> ChangeNotification { get; private set; }

    public T Value
    {
        get;
        private set;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var role in this.Roles)
        {
            role.PropertyChanged -= this.Role_PropertyChanged;
        }
    }

    private void Calculate()
    {
        this.Value = this.Expression();
    }
    
    private void Role_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        this.Calculate();
        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.PropertyName));
    }

}

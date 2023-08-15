namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class RoleExpressionAdapter<TObject, TValue> : IDisposable
    where TObject : IObject
{
    private IUnitRole<TValue> role;
    private TValue value;

    public RoleExpressionAdapter(IPropertyChange viewModel, IExpression<TObject, IUnitRole<TValue>> expression)
    {
        this.Expression = expression;
        this.ChangeNotification = new WeakReference<IPropertyChange>(viewModel);

        this.Expression.PropertyChanged += this.Expression_PropertyChanged;
    }

    public IExpression<TObject, IUnitRole<TValue>> Expression { get; }

    public IUnitRole<TValue> Role
    {
        get
        {
            if (this.role == null)
            {
                this.role = this.Expression.Value;
                if (role != null)
                {
                    this.role.PropertyChanged += Role_PropertyChanged;
                }
            }

            return role;
        }
    }

    public WeakReference<IPropertyChange> ChangeNotification { get; }

    public TValue Value
    {
        get => this.Role != null ? this.Role.Value : default;
        set
        {
            if (this.Role != null)
            {
                this.Role.Value = value;
            }
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        this.Expression.PropertyChanged += this.Expression_PropertyChanged;
        if (this.role != null)
        {
            this.role.PropertyChanged -= this.Role_PropertyChanged;
        }
    }

    private void Expression_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        if (this.role != null)
        {
            this.role.PropertyChanged -= this.Role_PropertyChanged;
            this.role = null;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Role.RoleType.Name));
    }
}

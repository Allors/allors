namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class UnitRoleExpressionAdapter<TObject, TValue> : IDisposable
    where TObject : IObject
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IExpression<TObject, IUnitRole<TValue>> expression;
    private readonly string name;

    private bool firstTime;
    private IUnitRole<TValue> role;
    private TValue value;

    public UnitRoleExpressionAdapter(IViewModel viewModel, IExpression<TObject, IUnitRole<TValue>> expression, string name)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.expression = expression;
        this.name = name;

        this.firstTime = true;
        this.expression.PropertyChanged += this.Expression_PropertyChanged;
    }

    public IUnitRole<TValue> Role
    {
        get
        {
            if (this.role == null)
            {
                this.role = this.expression.Value;
                if (this.role != null)
                {
                    this.role.PropertyChanged += this.Role_PropertyChanged;
                }
            }

            return this.role;
        }
    }

    public TValue Value
    {
        get
        {
            if (this.firstTime)
            {
                this.firstTime = false;

                if (this.Role != null)
                {
                    this.value = this.Role.Value;
                }
            }
            else
            {
                this.Evaluate();
            }

            return this.value;
        }
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

        this.expression.PropertyChanged += this.Expression_PropertyChanged;
        if (this.role != null)
        {
            this.role.PropertyChanged -= this.Role_PropertyChanged;
        }
    }

    private void Expression_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var newRole = this.expression.Value;
        if (!Equals(newRole, role))
        {
            if (this.role != null)
            {
                this.role.PropertyChanged -= this.Role_PropertyChanged;
            }

            this.role = newRole;

            if (this.role != null)
            {
                this.role.PropertyChanged += this.Role_PropertyChanged;
            }

            this.Evaluate();
        }
    }

    private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.Evaluate();
    }

    private void Evaluate()
    {
        if (!this.weakViewModel.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        if (this.role == null)
        {
            if (!Equals(this.value, default(TValue)))
            {
                this.value = default;
                changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.name));
            }
        }
        else
        {
            if (!Equals(this.value, this.role.Value))
            {
                this.value = this.role.Value;
                changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.name));
            }
        }
    }
}

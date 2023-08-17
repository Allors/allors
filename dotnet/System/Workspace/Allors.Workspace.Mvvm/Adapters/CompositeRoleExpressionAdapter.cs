namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class CompositeRoleExpressionAdapter<TObject, TComposite> : IDisposable
    where TObject : class, IObject
    where TComposite : class, IObject
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IExpression<TObject, ICompositeRole<TComposite>> expression;
    private readonly string name;

    private bool firstTime;
    private ICompositeRole<TComposite> role;
    private TComposite value;

    public CompositeRoleExpressionAdapter(IViewModel viewModel, IExpression<TObject, ICompositeRole<TComposite>> expression, string name)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.expression = expression;
        this.name = name;

        this.firstTime = true;
        this.expression.PropertyChanged += this.Expression_PropertyChanged;
    }

    public ICompositeRole<TComposite> Role
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

    public TComposite Value
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
            if (!Equals(this.value, default(TComposite)))
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

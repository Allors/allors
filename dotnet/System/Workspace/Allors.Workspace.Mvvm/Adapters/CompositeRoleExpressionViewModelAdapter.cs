namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class CompositeRoleExpressionViewModelAdapter<TObject, TViewModel> : IDisposable
    where TObject : class, IObject
    where TViewModel : class, IObjectViewModel<TObject>
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IViewModelResolver resolver;
    private readonly IExpression<TObject, ICompositeRole<TObject>> expression;
    private readonly string name;

    private bool firstTime;
    private ICompositeRole<TObject> role;
    private TObject value;

    private TViewModel cache;

    public CompositeRoleExpressionViewModelAdapter(IViewModel viewModel, IViewModelResolver resolver, IExpression<TObject, ICompositeRole<TObject>> expression, string name)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.resolver = resolver;
        this.expression = expression;
        this.name = name;

        this.firstTime = true;
        this.expression.PropertyChanged += this.Expression_PropertyChanged;
    }

    public ICompositeRole<TObject> Role
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

    public TViewModel Value
    {
        get
        {
            if (this.cache != null)
            {
                return this.cache;
            }

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

            return this.cache = (TViewModel)this.resolver.Resolve(this.value);
        }
        set
        {
            if (this.Role != null)
            {
                this.Role.Value = value.Model;
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
        this.cache = null;

        if (!this.weakViewModel.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        if (this.role == null)
        {
            if (!Equals(this.value, default(TObject)))
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

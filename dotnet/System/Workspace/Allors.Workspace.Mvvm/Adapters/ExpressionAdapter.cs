namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class ExpressionAdapter<TObject, TValue> : IDisposable
    where TObject : class, IObject
{
    private readonly WeakReference<IViewModel> weakViewModel;
    private readonly IExpression<TObject, TValue> expression;
    private readonly string name;

    public ExpressionAdapter(IViewModel viewModel, IExpression<TObject, TValue> expression, string name)
    {
        this.weakViewModel = new WeakReference<IViewModel>(viewModel);
        this.expression = expression;
        this.name = name;

        this.expression.PropertyChanged += this.Expression_PropertyChanged;
    }
    
    public TValue Value
    {
        get => this.expression.Value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.expression.PropertyChanged -= this.Expression_PropertyChanged;
    }

    private void Expression_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.weakViewModel.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.name));
    }
}

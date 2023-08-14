namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.ComponentModel;
using Allors.Workspace;

public class ExpressionAdapter<TObject, TValue> : IDisposable
    where TObject : IObject
{
    public ExpressionAdapter(IPropertyChange viewModel, IExpression<TObject, TValue> expression, string name)
    {
        this.Expression = expression;
        this.Name = name;
        this.ChangeNotification = new WeakReference<IPropertyChange>(viewModel);

        this.Expression.PropertyChanged += this.Expression_PropertyChanged;
    }

    public IExpression<TObject, TValue> Expression { get; }

    public string Name { get; }

    public WeakReference<IPropertyChange> ChangeNotification { get; }

    public TValue Value
    {
        get => this.Expression.Value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Expression.PropertyChanged -= this.Expression_PropertyChanged;
    }

    private void Expression_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!this.ChangeNotification.TryGetTarget(out var changeNotification))
        {
            this.Dispose();
            return;
        }

        changeNotification.OnPropertyChanged(new PropertyChangedEventArgs(this.Name));
    }
}

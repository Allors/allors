namespace Allors.Workspace.Mvvm.Adapters;

using System;
using System.Linq.Expressions;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Meta;

public static class IStrategyExensions
{
    public static PathAdapter<TRole> PathAdapter<TRole, TObject>(this IObject @object, Expression<Func<TObject, IRelationEndType>> path, IPropertyChange propertyChange, string propertyName) where TObject : IComposite
        => new PathAdapter<TRole>(@object.Strategy, path.Node(@object.Strategy.Class.MetaPopulation), new WeakReference<IPropertyChange>(propertyChange), propertyName);

    public static PathAdapter<T> PathAdapter<T>(this IStrategy strategy, Node path, WeakReference<IPropertyChange> propertyChange, string propertyName)
        => new PathAdapter<T>(strategy, path, propertyChange, propertyName);
}

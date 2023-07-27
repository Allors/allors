namespace Workspace.WinForms.ViewModels.Features;

using System.ComponentModel;
using System.Linq.Expressions;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Meta;
using Controllers;

public static class ICompositeExtensions
{
    public static RoleAdapter<TObject> PathAdapter<TComposite, TObject, TRole>(this TComposite objectType,
        IViewModel<TObject> viewModel, Func<TComposite, IEnumerable<Node>> nodes)
        where TComposite : IComposite
        where TObject : IObject
        where TRole : IObject
        => null; // new PathAdapter<TRole>(viewModel);
}

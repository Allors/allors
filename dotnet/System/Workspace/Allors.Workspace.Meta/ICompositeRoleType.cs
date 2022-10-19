namespace Allors.Workspace.Meta
{
    public interface ICompositeRoleType : IMetaExtensible
    {
        IRoleType RoleType { get; }

        IComposite Composite { get; }
    }
}

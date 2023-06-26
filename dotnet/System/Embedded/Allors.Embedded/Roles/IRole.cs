namespace Allors.Embedded
{
    using Meta;

    public interface IRole
    {
        IEmbeddedObject Object { get; }

        IEmbeddedRoleType RoleType { get; }
    }
}

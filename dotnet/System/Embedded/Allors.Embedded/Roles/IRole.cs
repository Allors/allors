namespace Allors.Embedded
{
    using Meta;

    public interface IRole
    {
        IEmbeddedObject Object { get; }

        EmbeddedRoleType RoleType { get; }
    }
}

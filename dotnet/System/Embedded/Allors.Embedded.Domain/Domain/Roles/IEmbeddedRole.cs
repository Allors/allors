namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedRole
    {
        IEmbeddedObject EmbeddedObject { get; }

        EmbeddedRoleType EmbeddedRoleType { get; }
    }
}

namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedAssociation
    {
        IEmbeddedObject EmbeddedObject { get; }

        EmbeddedAssociationType EmbeddedAssociationType { get; }
    }
}

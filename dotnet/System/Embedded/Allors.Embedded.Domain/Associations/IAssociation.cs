namespace Allors.Embedded
{
    using Meta;

    public interface IAssociation
    {
        IEmbeddedObject Object { get; }

        EmbeddedAssociationType AssociationType { get; }
    }
}

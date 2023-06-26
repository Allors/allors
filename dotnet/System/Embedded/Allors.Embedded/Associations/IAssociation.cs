namespace Allors.Embedded
{
    using Meta;

    public interface IAssociation
    {
        IEmbeddedObject Object { get; }

        IEmbeddedAssociationType AssociationType { get; }
    }
}

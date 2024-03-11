namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedObject
    {
        IEmbeddedPopulation EmbeddedPopulation { get; }

        EmbeddedObjectType EmbeddedObjectType { get; }
    }
}

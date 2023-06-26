namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedObject
    {
        IEmbeddedPopulation Population { get; }

        EmbeddedObjectType ObjectType { get; }
    }
}

namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedObject
    {
        IEmbeddedPopulation Population { get; }

        IEmbeddedObjectType ObjectType { get; }
    }
}

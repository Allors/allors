namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedObject
    {
        EmbeddedPopulation Population { get; }

        EmbeddedObjectType ObjectType { get; }

        UnitRole<T> GetUnitRole<T>(string name) ;

        CompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject ;

        CompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject;

        CompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject;

        CompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject;
    }
}

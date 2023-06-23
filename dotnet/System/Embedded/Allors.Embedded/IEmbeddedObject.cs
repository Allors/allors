namespace Allors.Embedded
{
    using Meta;

    public interface IEmbeddedObject
    {
        EmbeddedPopulation Population { get; }

        EmbeddedObjectType ObjectType { get; }

        UnitRole<T> GetUnitRole<T>(string name) ;

        UnitRole<T> GetUnitRole<T>(IEmbeddedRoleType roleType);

        CompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject ;

        CompositeRole<T> GetCompositeRole<T>(IEmbeddedRoleType roleType) where T : IEmbeddedObject;

        CompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject;

        CompositesRole<T> GetCompositesRole<T>(IEmbeddedRoleType roleType) where T : IEmbeddedObject;

        CompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject;

        CompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedAssociationType associationType) where T : IEmbeddedObject;

        CompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject;
        
        CompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedAssociationType associationType) where T : IEmbeddedObject;
    }
}

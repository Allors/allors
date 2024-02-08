namespace Allors.Embedded.Domain
{
    using Meta;

    public class EmbeddedObject : IEmbeddedObject
    {
        protected EmbeddedObject(IEmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.EmbeddedPopulation = population;
            this.EmbeddedObjectType = objectType;
        }

        public IEmbeddedPopulation EmbeddedPopulation { get; }

        public EmbeddedObjectType EmbeddedObjectType { get; }

        public IEmbeddedUnitRole<T> GetUnitRole<T>(string name) => this.GetUnitRole<T>(this.EmbeddedObjectType.EmbeddedRoleTypeByName[name]);

        public IEmbeddedUnitRole<T> GetUnitRole<T>(EmbeddedRoleType roleType) => this.EmbeddedPopulation.EmbeddedGetUnitRole<T>(this, roleType);

        public IEmbeddedCompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject => this.GetCompositeRole<T>(this.EmbeddedObjectType.EmbeddedRoleTypeByName[name]);

        public IEmbeddedCompositeRole<T> GetCompositeRole<T>(EmbeddedRoleType roleType) where T : IEmbeddedObject => this.EmbeddedPopulation.EmbeddedGetCompositeRole<T>(this, roleType);

        public IEmbeddedCompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject => this.GetCompositesRole<T>(this.EmbeddedObjectType.EmbeddedRoleTypeByName[name]);

        public IEmbeddedCompositesRole<T> GetCompositesRole<T>(EmbeddedRoleType roleType) where T : IEmbeddedObject => this.EmbeddedPopulation.EmbeddedGetCompositesRole<T>(this, roleType);

        public IEmbeddedCompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositeAssociation<T>(this.EmbeddedObjectType.EmbeddedAssociationTypeByName[name]);

        public IEmbeddedCompositeAssociation<T> GetCompositeAssociation<T>(EmbeddedAssociationType associationType) where T : IEmbeddedObject => this.EmbeddedPopulation.EmbeddedGetCompositeAssociation<T>(this, associationType);

        public IEmbeddedCompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositesAssociation<T>(this.EmbeddedObjectType.EmbeddedAssociationTypeByName[name]);

        public IEmbeddedCompositesAssociation<T> GetCompositesAssociation<T>(EmbeddedAssociationType associationType) where T : IEmbeddedObject => this.EmbeddedPopulation.EmbeddedGetCompositesAssociation<T>(this, associationType);
    }
}

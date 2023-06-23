namespace Allors.Embedded
{
    using Meta;

    public class EmbeddedObject : IEmbeddedObject
    {
        protected EmbeddedObject(EmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.Population = population;
            this.ObjectType = objectType;
        }

        public EmbeddedPopulation Population { get; }

        public EmbeddedObjectType ObjectType { get; }
        
        public UnitRole<T> GetUnitRole<T>(string name) => this.Population.GetUnitRole<T>(this, this.ObjectType.RoleTypeByName[name]);

        public CompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositeRole<T>(this, this.ObjectType.RoleTypeByName[name]);

        public CompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositesRole<T>(this, this.ObjectType.RoleTypeByName[name]);

        public CompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositeAssociation<T>(this, this.ObjectType.AssociationTypeByName[name]);

        public CompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositesAssociation<T>(this, this.ObjectType.AssociationTypeByName[name]);
    }
}

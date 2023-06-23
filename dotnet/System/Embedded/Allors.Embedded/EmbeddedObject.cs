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

        public UnitRole<T> GetUnitRole<T>(string name) => this.GetUnitRole<T>(this.ObjectType.RoleTypeByName[name]);

        public UnitRole<T> GetUnitRole<T>(IEmbeddedRoleType roleType) => this.Population.GetUnitRole<T>(this, roleType);

        public CompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject => this.GetCompositeRole<T>(this.ObjectType.RoleTypeByName[name]);

        public CompositeRole<T> GetCompositeRole<T>(IEmbeddedRoleType roleType) where T : IEmbeddedObject => this.Population.GetCompositeRole<T>(this, roleType);

        public CompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject => this.GetCompositesRole<T>(this.ObjectType.RoleTypeByName[name]);

        public CompositesRole<T> GetCompositesRole<T>(IEmbeddedRoleType roleType) where T : IEmbeddedObject => this.Population.GetCompositesRole<T>(this, roleType);

        public CompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositeAssociation<T>(this.ObjectType.AssociationTypeByName[name]);

        public CompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedAssociationType associationType) where T : IEmbeddedObject => this.Population.GetCompositeAssociation<T>(this, associationType);

        public CompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositesAssociation<T>(this.ObjectType.AssociationTypeByName[name]);

        public CompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedAssociationType associationType) where T : IEmbeddedObject => this.Population.GetCompositesAssociation<T>(this, associationType);
    }
}

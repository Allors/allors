namespace Allors.Embedded.Domain
{
    using Meta;

    public class EmbeddedObject : IEmbeddedObject
    {
        protected EmbeddedObject(IEmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.Population = population;
            this.ObjectType = objectType;
        }

        public IEmbeddedPopulation Population { get; }

        public EmbeddedObjectType ObjectType { get; }

        public IUnitRole<T> GetUnitRole<T>(string name) => this.GetUnitRole<T>(this.ObjectType.RoleTypeByName[name]);

        public IUnitRole<T> GetUnitRole<T>(EmbeddedRoleType roleType) => this.Population.GetUnitRole<T>(this, roleType);

        public ICompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject => this.GetCompositeRole<T>(this.ObjectType.RoleTypeByName[name]);

        public ICompositeRole<T> GetCompositeRole<T>(EmbeddedRoleType roleType) where T : IEmbeddedObject => this.Population.GetCompositeRole<T>(this, roleType);

        public ICompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject => this.GetCompositesRole<T>(this.ObjectType.RoleTypeByName[name]);

        public ICompositesRole<T> GetCompositesRole<T>(EmbeddedRoleType roleType) where T : IEmbeddedObject => this.Population.GetCompositesRole<T>(this, roleType);

        public ICompositeAssociation<T> GetCompositeAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositeAssociation<T>(this.ObjectType.AssociationTypeByName[name]);

        public ICompositeAssociation<T> GetCompositeAssociation<T>(EmbeddedAssociationType associationType) where T : IEmbeddedObject => this.Population.GetCompositeAssociation<T>(this, associationType);

        public ICompositesAssociation<T> GetCompositesAssociation<T>(string name) where T : IEmbeddedObject => this.GetCompositesAssociation<T>(this.ObjectType.AssociationTypeByName[name]);

        public ICompositesAssociation<T> GetCompositesAssociation<T>(EmbeddedAssociationType associationType) where T : IEmbeddedObject => this.Population.GetCompositesAssociation<T>(this, associationType);
    }
}

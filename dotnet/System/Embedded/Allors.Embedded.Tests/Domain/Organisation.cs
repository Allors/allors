namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;
    using Newtonsoft.Json.Linq;

    public class Organization : EmbeddedObject, INamed
    {
        public Organization(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetUnitRole<string>("Name");
            this.Named = GetCompositeRole<INamed>("Named");
            this.Owner = GetCompositeRole<Person>("Owner");
            this.Employees = GetCompositesRole<Person>("Employee");
        }

        public UnitRole<string> Name { get; }

        public CompositeRole<INamed> Named { get; }

        public CompositeRole<Person> Owner { get; }

        public CompositesRole<Person> Employees { get; }

        public Organization OrganizationWhereNamed => (Organization)this.GetAssociationValue(nameof(OrganizationWhereNamed));
    }
}

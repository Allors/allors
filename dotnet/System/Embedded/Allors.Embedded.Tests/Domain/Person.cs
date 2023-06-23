namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Person : EmbeddedObject, INamed
    {
        public Person(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetUnitRole<string>("Name");
            this.FirstName = GetUnitRole<string>("FirstName");
            this.LastName = GetUnitRole<string>("LastName");
            this.FullName = GetUnitRole<string>("FullName");
            this.DerivedAt = GetUnitRole<DateTime>("DerivedAt");
            this.Greeting = GetUnitRole<string>("Greeting");

            this.OrganizationWhereOwner = GetCompositeAssociation<Organization>("OrganizationWhereOwner");
            this.OrganizationsWhereEmployee = GetCompositesAssociation<Organization>("OrganizationsWhereEmployee");
            this.OrganizationWhereNamed = GetCompositeAssociation<Organization>("OrganizationWhereNamed");
        }

        public IUnitRole<string> Name { get; }

        public IUnitRole<string> FirstName { get; }

        public IUnitRole<string> LastName { get; }

        public IUnitRole<string> FullName { get; }

        public IUnitRole<DateTime> DerivedAt { get; }

        public IUnitRole<string> Greeting { get; }
        
        public CompositeAssociation<Organization> OrganizationWhereOwner { get; }

        public CompositesAssociation<Organization> OrganizationsWhereEmployee { get; }

        public CompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

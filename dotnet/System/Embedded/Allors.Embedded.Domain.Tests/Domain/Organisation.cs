namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Organization : EmbeddedObject, INamed
    {
        public Organization(IEmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetUnitRole<string>("Name");
            this.Named = GetCompositeRole<INamed>("Named");
            this.UppercasedName = GetUnitRole<string>("UppercasedName");
            this.Aliases = GetUnitRole<string[]>("Aliases");
            this.Owner = GetCompositeRole<Person>("Owner");
            this.Employees = GetCompositesRole<Person>("Employee");

            this.OrganizationWhereNamed = GetCompositeAssociation<Organization>("OrganizationWhereNamed");
        }
       
        public IUnitRole<string> Name { get; }

        public IUnitRole<string> UppercasedName { get; }
        
        public ICompositeRole<INamed> Named { get; }

        public IUnitRole<string[]> Aliases { get; }

        public ICompositeRole<Person> Owner { get; }

        public ICompositesRole<Person> Employees { get; }

        public ICompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

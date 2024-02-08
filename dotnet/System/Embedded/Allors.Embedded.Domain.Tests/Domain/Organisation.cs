namespace Allors.Embedded.Domain
{
    using Allors.Embedded.Meta;

    public class Organization : EmbeddedObject, INamed
    {
        public Organization(IEmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetUnitRole<string>(nameof(Name));
            this.Named = GetCompositeRole<INamed>(nameof(Named));
            this.UppercasedName = GetUnitRole<string>(nameof(UppercasedName));
            this.Aliases = GetUnitRole<string[]>(nameof(Aliases));
            this.DisplayAliases = GetUnitRole<string>(nameof(DisplayAliases));
            this.Owner = GetCompositeRole<Person>(nameof(Owner));
            this.Employees = GetCompositesRole<Person>(nameof(Employees));

            this.OrganizationWhereNamed = GetCompositeAssociation<Organization>(nameof(OrganizationWhereNamed));
        }
       
        public IEmbeddedUnitRole<string> Name { get; }

        public IEmbeddedUnitRole<string> UppercasedName { get; }
        
        public IEmbeddedCompositeRole<INamed> Named { get; }

        public IEmbeddedUnitRole<string[]> Aliases { get; }

        public IEmbeddedUnitRole<string> DisplayAliases { get; }

        public IEmbeddedCompositeRole<Person> Owner { get; }

        public IEmbeddedCompositesRole<Person> Employees { get; }

        public IEmbeddedCompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

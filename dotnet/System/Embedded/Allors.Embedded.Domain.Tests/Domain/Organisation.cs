namespace Allors.Embedded.Tests.Domain
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
       
        public IUnitRole<string> Name { get; }

        public IUnitRole<string> UppercasedName { get; }
        
        public ICompositeRole<INamed> Named { get; }

        public IUnitRole<string[]> Aliases { get; }

        public IUnitRole<string> DisplayAliases { get; }

        public ICompositeRole<Person> Owner { get; }

        public ICompositesRole<Person> Employees { get; }

        public ICompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Person : EmbeddedObject, INamed
    {
        public Person(IEmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetUnitRole<string>(nameof(Name));
            this.UppercasedName = GetUnitRole<string>(nameof(UppercasedName));
            this.FirstName = GetUnitRole<string>(nameof(FirstName));
            this.LastName = GetUnitRole<string>(nameof(LastName));
            this.FullName = GetUnitRole<string>(nameof(FullName));
            this.DerivedAt = GetUnitRole<DateTime>(nameof(DerivedAt));
            this.Greeting = GetUnitRole<string>(nameof(Greeting));

            this.OrganizationWhereOwner = GetCompositeAssociation<Organization>(nameof(OrganizationWhereOwner));
            this.OrganizationsWhereEmployee = GetCompositesAssociation<Organization>(nameof(OrganizationsWhereEmployee));
            this.OrganizationWhereNamed = GetCompositeAssociation<Organization>(nameof(OrganizationWhereNamed));
        }

        public IUnitRole<string> Name { get; }

        public IUnitRole<string> UppercasedName { get; }

        public IUnitRole<string> FirstName { get; }

        public IUnitRole<string> LastName { get; }

        public IUnitRole<string> FullName { get; }

        public IUnitRole<DateTime> DerivedAt { get; }

        public IUnitRole<string> Greeting { get; }

        public ICompositeAssociation<Organization> OrganizationWhereOwner { get; }

        public ICompositesAssociation<Organization> OrganizationsWhereEmployee { get; }

        public ICompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

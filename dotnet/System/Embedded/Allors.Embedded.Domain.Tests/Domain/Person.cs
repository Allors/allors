namespace Allors.Embedded.Domain
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

        public IEmbeddedUnitRole<string> Name { get; }

        public IEmbeddedUnitRole<string> UppercasedName { get; }

        public IEmbeddedUnitRole<string> FirstName { get; }

        public IEmbeddedUnitRole<string> LastName { get; }

        public IEmbeddedUnitRole<string> FullName { get; }

        public IEmbeddedUnitRole<DateTime> DerivedAt { get; }

        public IEmbeddedUnitRole<string> Greeting { get; }

        public IEmbeddedCompositeAssociation<Organization> OrganizationWhereOwner { get; }

        public IEmbeddedCompositesAssociation<Organization> OrganizationsWhereEmployee { get; }

        public IEmbeddedCompositeAssociation<Organization> OrganizationWhereNamed { get; }
    }
}

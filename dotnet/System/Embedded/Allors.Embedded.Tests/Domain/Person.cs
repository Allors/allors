namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Person : EmbeddedObject, INamed
    {
        public Person(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
        }

        public string Name
        {
            get { return (string)this.GetRole(nameof(Name)); }
            set { this.SetRole(nameof(Name), value); }
        }

        public Organization Named
        {
            get { return (Organization)this.GetRole(nameof(Named)); }
            set { this.SetRole(nameof(Named), value); }
        }

        public string FirstName
        {
            get { return (string)this.GetRole(nameof(FirstName)); }
            set { this.SetRole(nameof(FirstName), value); }
        }

        public string LastName
        {
            get { return (string)this.GetRole(nameof(LastName)); }
            set { this.SetRole(nameof(LastName), value); }
        }

        public string FullName
        {
            get { return (string)this.GetRole(nameof(FullName)); }
            set { this.SetRole(nameof(FullName), value); }
        }

        public DateTime DerivedAt
        {
            get { return (DateTime)this.GetRole(nameof(DerivedAt)); }
            set { this.SetRole(nameof(DerivedAt), value); }
        }

        public string Greeting
        {
            get { return (string)this.GetRole(nameof(Greeting)); }
            set { this.SetRole(nameof(Greeting), value); }
        }

        public Organization OrganizationWhereOwner => (Organization)this.GetAssociation(nameof(OrganizationWhereOwner));

        public Organization[] OrganizationsWhereEmployee => ((EmbeddedObject[])this.GetAssociation(nameof(OrganizationsWhereEmployee)))?.Cast<Organization>().ToArray() ?? Array.Empty<Organization>();

        public Organization OrganizationWhereNamed => (Organization)this.GetAssociation(nameof(OrganizationWhereNamed));
    }
}

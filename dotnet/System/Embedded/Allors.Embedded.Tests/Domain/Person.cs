namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Person : EmbeddedObject, INamed
    {
        public Person(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetStringRole("Name");
        }

        public StringRole Name { get; }

      
        public Organization Named
        {
            get { return (Organization)this.GetRoleValue(nameof(Named)); }
            set { this.SetRoleValue(nameof(Named), value); }
        }

        public string FirstName
        {
            get { return (string)this.GetRoleValue(nameof(FirstName)); }
            set { this.SetRoleValue(nameof(FirstName), value); }
        }

        public string LastName
        {
            get { return (string)this.GetRoleValue(nameof(LastName)); }
            set { this.SetRoleValue(nameof(LastName), value); }
        }

        public string FullName
        {
            get { return (string)this.GetRoleValue(nameof(FullName)); }
            set { this.SetRoleValue(nameof(FullName), value); }
        }

        public DateTime DerivedAt
        {
            get { return (DateTime)this.GetRoleValue(nameof(DerivedAt)); }
            set { this.SetRoleValue(nameof(DerivedAt), value); }
        }

        public string Greeting
        {
            get { return (string)this.GetRoleValue(nameof(Greeting)); }
            set { this.SetRoleValue(nameof(Greeting), value); }
        }

        public Organization OrganizationWhereOwner => (Organization)this.GetAssociationValue(nameof(OrganizationWhereOwner));

        public Organization[] OrganizationsWhereEmployee => ((EmbeddedObject[])this.GetAssociationValue(nameof(OrganizationsWhereEmployee)))?.Cast<Organization>().ToArray() ?? Array.Empty<Organization>();

        public Organization OrganizationWhereNamed => (Organization)this.GetAssociationValue(nameof(OrganizationWhereNamed));
    }
}

namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;
    using Newtonsoft.Json.Linq;

    public class Organization : EmbeddedObject, INamed
    {
        public Organization(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
        }

        public string Name
        {
            get { return (string)this.GetRole(nameof(Name)); }
            set { this.SetRole(nameof(Name), value); }
        }

        public INamed Named
        {
            get { return (INamed)this.GetRole(nameof(Named)); }
            set { this.SetRole(nameof(Named), value); }
        }

        public Person Owner
        {
            get { return (Person)this.GetRole(nameof(Owner)); }
            set { this.SetRole(nameof(Owner), value); }
        }

        public Person[] Employees
        {
            get { return ((EmbeddedObject[])this.GetRole(nameof(Employees)))?.Cast<Person>().ToArray() ?? Array.Empty<Person>(); }
            set { this.SetRole(nameof(Employees), value); }
        }

        public void AddEmployee(Person value)
        {
            this.AddRole(nameof(Employees), value);
        }

        public void RemoveEmployee(Person value)
        {
            this.RemoveRole(nameof(Employees), value);
        }

        public Organization OrganizationWhereNamed => (Organization)this.GetAssociation(nameof(OrganizationWhereNamed));
    }
}

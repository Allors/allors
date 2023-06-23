namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;
    using Newtonsoft.Json.Linq;

    public class Organization : EmbeddedObject, INamed
    {
        public Organization(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Name = GetStringRole("Name");
        }

        public StringRole Name { get; }

        public INamed Named
        {
            get { return (INamed)this.GetRoleValue(nameof(Named)); }
            set { this.SetRoleValue(nameof(Named), value); }
        }

        public Person Owner
        {
            get { return (Person)this.GetRoleValue(nameof(Owner)); }
            set { this.SetRoleValue(nameof(Owner), value); }
        }

        public Person[] Employees
        {
            get { return ((EmbeddedObject[])this.GetRoleValue(nameof(Employees)))?.Cast<Person>().ToArray() ?? Array.Empty<Person>(); }
            set { this.SetRoleValue(nameof(Employees), value); }
        }

        public void AddEmployee(Person value)
        {
            this.AddRoleValue(nameof(Employees), value);
        }

        public void RemoveEmployee(Person value)
        {
            this.RemoveRoleValue(nameof(Employees), value);
        }

        public Organization OrganizationWhereNamed => (Organization)this.GetAssociationValue(nameof(OrganizationWhereNamed));
    }
}

namespace Allors.Embedded.Tests
{
    using Domain;

    public abstract class Tests
    {
        public EmbeddedPopulation Population { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Population = new EmbeddedPopulation(
                v =>
                {
                    v.AddUnit<INamed, string>(nameof(INamed.Name));
                    v.AddUnit<INamed, string>(nameof(INamed.UppercasedName));
                    v.AddOneToOne<Organization, INamed>(nameof(Organization.Named));
                    v.AddOneToOne<Organization, Person>(nameof(Organization.Owner));
                    v.AddManyToMany<Organization, Person>("Employee");
                    v.AddUnit<Person, string>(nameof(Person.FirstName));
                    v.AddUnit<Person, string>(nameof(Person.LastName));
                    v.AddUnit<Person, string>(nameof(Person.FullName));
                    v.AddUnit<Person, DateTime>(nameof(Person.DerivedAt));
                    v.AddUnit<Person, string>(nameof(Person.Greeting));
                    // Special
                    v.AddUnit<C1, string>(nameof(C1.Same));
                    v.AddUnit<C2, string>(nameof(C2.Same));
                });
        }
    }
}

namespace Allors.Embedded.Tests
{
    using Domain;

    public abstract class Tests
    {
        public abstract IEmbeddedPopulation Population { get; }

        public virtual void SetUp()
        {
            var meta = this.Population.Meta;

            meta.AddUnit<INamed, string>(nameof(INamed.Name));
            meta.AddUnit<INamed, string>(nameof(INamed.UppercasedName));
            meta.AddOneToOne<Organization, INamed>(nameof(Organization.Named));
            meta.AddUnit<Organization, string[]>(nameof(Organization.Aliases));
            meta.AddUnit<INamed, string>(nameof(Organization.DisplayAliases));
            meta.AddOneToOne<Organization, Person>(nameof(Organization.Owner));
            meta.AddManyToMany<Organization, Person>("Employee");
            meta.AddUnit<Person, string>(nameof(Person.FirstName));
            meta.AddUnit<Person, string>(nameof(Person.LastName));
            meta.AddUnit<Person, string>(nameof(Person.FullName));
            meta.AddUnit<Person, DateTime>(nameof(Person.DerivedAt));
            meta.AddUnit<Person, string>(nameof(Person.Greeting));
            // Special
            meta.AddUnit<C1, string>(nameof(C1.Same));
            meta.AddUnit<C2, string>(nameof(C2.Same));
        }
    }
}

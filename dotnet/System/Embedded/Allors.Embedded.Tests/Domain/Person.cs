namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Person : EmbeddedObject
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

    }
}

namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class Organisation : EmbeddedObject
    {
        public Organisation(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
        }

        public string Name
        {
            get { return (string)this.GetRole(nameof(Name)); }
            set { this.SetRole(nameof(Name), value); }
        }
        
        public Person Owner
        {
            get { return (Person)this.GetRole(nameof(Owner)); }
            set { this.SetRole(nameof(Owner), value); }
        }
    }
}

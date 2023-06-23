namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class C1 : EmbeddedObject
    {
        public C1(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Same = GetUnitRole<string>("Same");
        }

        public UnitRole<string> Same { get; }
    }
}

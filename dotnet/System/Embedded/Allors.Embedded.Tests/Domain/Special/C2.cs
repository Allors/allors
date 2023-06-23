namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class C2 : EmbeddedObject
    {
        public C2(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Same = GetUnitRole<string>("Same");
        }

        public UnitRole<string> Same { get; }
    }
}

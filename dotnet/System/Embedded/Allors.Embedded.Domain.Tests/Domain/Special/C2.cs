namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class C2 : EmbeddedObject, I2
    {
        public C2(IEmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.Backs = GetCompositesAssociation<I1>(nameof(this.Backs));
            this.Same = GetUnitRole<string>(nameof(Same));
        }

        public ICompositesAssociation<I1> Backs { get; }

        public IUnitRole<string> Same { get; }
    }
}

namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class C1 : EmbeddedObject, I1
    {
        public C1(IEmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
            this.ManyToMany = GetCompositesRole<I2>(nameof(this.ManyToMany));
            this.Same = GetUnitRole<string>(nameof(Same));
        }
        public ICompositesRole<I2> ManyToMany { get; }
        
        public IUnitRole<string> Same { get; }
    }
}
